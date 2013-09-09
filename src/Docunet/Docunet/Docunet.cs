using System;
using System.Collections.Generic;

namespace Docunet
{
    public class Docunet : Dictionary<string, object>
    {
        #region Field getters
        
        public bool Bool(string fieldPath)
        {
            var value = GetField(fieldPath);
            
            if (value == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (bool)value;
            }
        }
        
        public byte Byte(string fieldPath)
        {
            var value = GetField(fieldPath);
            
            if (value == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (byte)value;
            }
        }
        
        public short Short(string fieldPath)
        {
            var value = GetField(fieldPath);
            
            if (value == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (short)value;
            }
        }
        
        public int Int(string fieldPath)
        {
            var value = GetField(fieldPath);
            
            if (value == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (int)value;
            }
        }
        
        public long Long(string fieldPath)
        {
            var value = GetField(fieldPath);
            
            if (value == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (long)value;
            }
        }
        
        public float Float(string fieldPath)
        {
            var value = GetField(fieldPath);
            
            if (value == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (float)value;
            }
        }
        
        public double Double(string fieldPath)
        {
            var value = GetField(fieldPath);
            
            if (value == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (double)value;
            }
        }
        
        public decimal Decimal(string fieldPath)
        {
            var value = GetField(fieldPath);
            
            if (value == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return (decimal)value;
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
        
        // used for null value
        public Docunet Object(string fieldPath, object value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Object<T>(string fieldPath, T value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Document<T>(string fieldPath, T value)
        {
            if (value is Docunet)
            {
                SetField(fieldPath, value);
            }
            else
            {
                // TODO: if value is other than Docunet, parse its content to docunet type
                SetField(fieldPath, Docunet.ToDocument<T>(value));
            }
            
            return this;
        }
        
        public Docunet List<T>(string fieldPath, List<T> value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        private void SetField(string fieldPath, object value)
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
                            embeddedDocument[field] = value;
                        }
                        else
                        {
                            embeddedDocument.Add(field, value);
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
                    this[fieldPath] = value;
                }
                else
                {
                    this.Add(fieldPath, value);
                }
            }
        }
        
        #endregion
        
        public static Docunet ToDocument<T>(T value)
        {
            if (value is Docunet)
            {
                return value as Docunet;
            }
            else if (value is Dictionary<string, object>)
            {
                var document = new Docunet();
                
                foreach (KeyValuePair<string, object> field in value as Dictionary<string, object>)
                {
                    document.Object(field.Key, field.Value);
                }
                
                return document;
            }
            else
            {
                var valueType = value.GetType();
                var document = new Docunet();
                
                
                
                return document;
            }
        }
    }
}