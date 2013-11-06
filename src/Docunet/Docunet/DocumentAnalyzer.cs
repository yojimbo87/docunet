using System;
using System.Collections.Generic;

namespace Docunet
{
    /// <summary>
    /// Analyzes document according to specific rules.
    /// </summary>
    public class DocumentAnalyzer
    {
        private Document _document;
        private Dictionary<string, Type> _shouldHaveFields = new Dictionary<string, Type>();
        private Dictionary<string, Type> _mustHaveFields = new Dictionary<string, Type>();
        
        /// <summary> 
        /// Creates analyzer which will analyze specified document object.
        /// </summary>
        /// <param name="document">Document object which will be analyzed.</param>
        public DocumentAnalyzer(Document document)
        {
            _document = document;
        }
        
        /// <summary> 
        /// Specifies field which if present should be of specified type.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="fieldType">Type of which the field must be.</param>
        public DocumentAnalyzer ShouldHave(string fieldPath, Type fieldType)
        {
            _shouldHaveFields.Add(fieldPath, fieldType);
            
            return this;
        }
        
        /// <summary> 
        /// Specifies field which must be present in document.
        /// </summary>
        /// <param name="fieldPaths">Path to the fields in document.</param>
        public DocumentAnalyzer MustHave(params string[] fieldPaths)
        {
            foreach (string fieldPath in fieldPaths)
            {
                _mustHaveFields.Add(fieldPath, null);
            }
            
            return this;
        }
        
        /// <summary> 
        /// Specifies field which must be present and must be of specified type.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="fieldType">Type of which the field must be.</param>
        public DocumentAnalyzer MustHave(string fieldPath, Type fieldType)
        {
            _mustHaveFields.Add(fieldPath, fieldType);
            
            return this;
        }
        
        /// <summary> 
        /// Analyzes document according to previously set rules.
        /// </summary>
        public bool Analyze()
        {
            if (_shouldHaveFields.Count > 0)
            {
                foreach (KeyValuePair<string, Type> shouldHaveField in _shouldHaveFields)
                {
                    if (_document.Has(shouldHaveField.Key))
                    {
                        if (!_document.Has(shouldHaveField.Key, shouldHaveField.Value))
                        {
                            return false;
                        }
                    }
                }
            }
            
            if (_mustHaveFields.Count > 0)
            {
                foreach (KeyValuePair<string, Type> mustHaveField in _mustHaveFields)
                {
                    if (mustHaveField.Value == null)
                    {
                        if (!_document.Has(mustHaveField.Key))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!_document.Has(mustHaveField.Key, mustHaveField.Value))
                        {
                            return false;
                        }
                    }
                }
            }
            
            return true;
        }
    }
}
