using System;

namespace KTInsta.Entities
{
    public abstract class BaseModel
    {
        public BaseModel()
        {
            InitializeDefaults();
        }

        public virtual string ToXml()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A short string representing this class that is understandable by the support team.
        /// </summary>
        /// <returns></returns>
        public virtual string ToLabelForSupport()
        {
            return null;
        }

        public virtual void InitializeDefaults()
        {

        }
    }
}


