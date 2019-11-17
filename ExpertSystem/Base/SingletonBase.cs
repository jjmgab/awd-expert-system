using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Base
{
    public abstract class SingletonBase<T> where T : SingletonBase<T>
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        protected static T _instance;

        /// <summary>
        /// Flag indicating if the handler was already initialized.
        /// </summary>
        private bool _isInitialized = false;
        /// <summary>
        /// Flag indicating if the handler was already initialized.
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// Private constructor.
        /// </summary>
        protected SingletonBase() { }

        /// <summary>
        /// Reads and parses the JSON.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public bool Initialize(object[] args)
        {
            if (!_isInitialized)
            {
                Init(args);
                _isInitialized = true;
                return true;
            }
            return false;
        }

        protected abstract void Init(object[] args);

        /// <summary>
        /// Resets loader state to post-constructor.
        /// </summary>
        public virtual void ResetState()
        {
            _isInitialized = false;
        }

        /// <summary>
        /// Checks for validity of loader current state.
        /// Throws InvalidOperationException if invalid.
        /// </summary>
        protected void PedanticCheck()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Handler not yet initialized.");
            }
        }
    }
}
