using System;
using System.Collections.Generic;
using System.Reflection;

namespace Docunet
{
    public class Docunet : Dictionary<string, object>
    {
        #region Field getters
        
        public bool Bool(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (bool)fieldValue;
            }
        }
        
        public byte Byte(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (byte)fieldValue;
            }
        }
        
        public short Short(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (short)fieldValue;
            }
        }
        
        public int Int(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (int)fieldValue;
            }
        }
        
        public long Long(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (long)fieldValue;
            }
        }
        
        public float Float(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (float)fieldValue;
            }
        }
        
        public double Double(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (double)fieldValue;
            }
        }
        
        public decimal Decimal(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (decimal)fieldValue;
            }
        }
        
        public string String(string fieldPath)
        {
            return (string)GetField(fieldPath);
        }
        
        public DateTime DateTime(string fieldPath)
        {
            return (DateTime)GetField(fieldPath);
        }
        
        public object Object(string fieldPath)
        {
            return (object)GetField(fieldPath);
        }
        
        public T Object<T>(string fieldPath)
        {
            return (T)GetField(fieldPath);
        }
        
        public Docunet Document(string fieldPath)
        {
            return (Docunet)GetField(fieldPath);
        }
        
        public List<T> List<T>(string fieldPath)
        {
            return (List<T>)GetField(fieldPath);
        }
        
        private object GetField(string fieldPath)
        {
            if (fieldPath.Contains("."))
            {
                var fields = fieldPath.Split('.');
                var iteration = 1;
                var embeddedDocument = this;

                foreach (var field in fields)
                {
                    if (iteration == fields.Length)
                    {
                        if (embeddedDocument.ContainsKey(field))
                        {
                            return embeddedDocument[field];
                        }
                        
                        break;
                    }

                    if (embeddedDocument.ContainsKey(field))
                    {
                        embeddedDocument = (Docunet)embeddedDocument[field];
                    }
                    else
                    {
                        // if current field in path isn't present
                        break;
                    }

                    iteration++;
                }
            }
            else
            {
                if (this.ContainsKey(fieldPath))
                {
                    return this[fieldPath];
                }
            }
            
            return null;
        }
        
        #endregion
        
        #region Field setters
        
        public Docunet Bool(string fieldPath, bool value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Byte(string fieldPath, byte value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Short(string fieldPath, short value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Int(string fieldPath, int value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Long(string fieldPath, long value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Float(string fieldPath, float value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Double(string fieldPath, double value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Decimal(string fieldPath, decimal value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet String(string fieldPath, string value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet DateTime(string fieldPath, DateTime value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        // used for null inputObject
        public Docunet Object(string fieldPath, object inputObject)
        {
            SetField(fieldPath, inputObject);

            return this;
        }
        
        public Docunet Object<T>(string fieldPath, T inputObject)
        {
            SetField(fieldPath, inputObject);

            return this;
        }
        
        public Docunet Document<T>(string fieldPath, T inputObject)
        {
            if (inputObject is Docunet)
            {
                SetField(fieldPath, inputObject);
            }
            else
            {
                SetField(fieldPath, Docunet.ToDocument<T>(inputObject));
            }
            
            return this;
        }
        
        public Docunet List<T>(string fieldPath, List<T> inputObject)
        {
            SetField(fieldPath, inputObject);

            return this;
        }
        
        private void SetField(string fieldPath, object inputObject)
        {
            if (fieldPath.Contains("."))
            {
                var fields = fieldPath.Split('.');
                var iteration = 1;
                var embeddedDocument = this;

                foreach (var field in fields)
                {
                    if (iteration == fields.Length)
                    {
                        if (embeddedDocument.ContainsKey(field))
                        {
                            embeddedDocument[field] = inputObject;
                        }
                        else
                        {
                            embeddedDocument.Add(field, inputObject);
                        }
                        
                        break;
                    }

                    if (embeddedDocument.ContainsKey(field))
                    {
                        embeddedDocument = (Docunet)embeddedDocument[field];
                    }
                    else
                    {
                        // if document which contains the field doesn't exist create it first
                        var tempDocument = new Docunet();
                        embeddedDocument.Add(field, tempDocument);
                        embeddedDocument = tempDocument;
                    }

                    iteration++;
                }
            }
            else
            {
                if (this.ContainsKey(fieldPath))
                {
                    this[fieldPath] = inputObject;
                }
                else
                {
                    this.Add(fieldPath, inputObject);
                }
            }
        }
        
        #endregion
        
        public static Docunet ToDocument<T>(T inputObject)
        {
            if (inputObject is Docunet)
            {
                return inputObject as Docunet;
            }
            else if (inputObject is Dictionary<string, object>)
            {
                var document = new Docunet();
                
                foreach (KeyValuePair<string, object> field in inputObject as Dictionary<string, object>)
                {
                    document.Object(field.Key, field.Value);
                }
                
                return document;
            }
            else
            {
                var inputObjectType = inputObject.GetType();
                var document = new Docunet();
                
                foreach (var propertyInfo in inputObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var propertyValue = propertyInfo.GetValue(inputObject);
                    
                    if (propertyInfo.PropertyType.IsArray || propertyInfo.PropertyType.IsGenericType)
                    {
                        /*document.SetField(
                            propertyName, 
                            ConvertFromCollection((IList)propertyValue, propertyInfo.PropertyType)
                        );*/
                    }
                    // property is class except the string type since string values are parsed differently
                    else if (propertyInfo.PropertyType.IsClass && (propertyInfo.PropertyType.Name != "String"))
                    {
                        /*document.SetField(
                            propertyName, 
                            ConvertFromObject(propertyValue, propertyInfo.PropertyType)
                        );*/
                    }
                    // property is basic type
                    else
                    {
                        document.SetField(propertyInfo.Name, propertyValue);
                    }
                }
                
                return document;
            }
        }
        
        // TODO: implement ToList<T>(T inputObject) which converts inputObject to List<Docunet>
    }
}