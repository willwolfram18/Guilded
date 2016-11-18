using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Selama.Common.Http;

namespace Selama.Controllers
{
    /// <summary>
    /// Base controller from which all project controllers should derive
    /// </summary>
    [RequireHttps]
    public class _ControllerBase : Controller
    {
        #region Properties
        #region Public properties
        /// <summary>
        /// The current session
        /// </summary>
        public ICompleteSession Session
        {
            get
            {
                if (HttpContext.Session is ICompleteSession)
                {
                    _session = HttpContext.Session as ICompleteSession;
                }
                return _session ??
                    (_session = new CompleteSession(HttpContext.Session));
            }
        }
        #endregion

        #region Private properties
        /// <summary>
        /// Object behind <see cref="Session"/>
        /// </summary>
        private ICompleteSession _session;
        #endregion
        #endregion
    }
}