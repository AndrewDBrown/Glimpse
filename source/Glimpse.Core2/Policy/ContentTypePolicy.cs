﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
    [RuntimePolicy(RuntimeEvent.EndRequest)]
    public class ContentTypePolicy:ConfigurationSection, IRuntimePolicy
    {
        //TODO: Turn into a proper configuration class
        public IList<string> ContentTypeWhitelist { get; set; }

        public ContentTypePolicy()
        {
            ContentTypeWhitelist = new List<string>
                                       {
                                           @"text/html",
                                           @"application/json"
                                       };
        }

        public ContentTypePolicy(IList<string> contentTypeWhitelist)
        {
            Contract.Requires<ArgumentNullException>(contentTypeWhitelist != null, "contentTypeWhitelist");

            ContentTypeWhitelist = contentTypeWhitelist;
        }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            try
            {
                var contentType = policyContext.RequestMetadata.ResponseContentType;
                return ContentTypeWhitelist.Contains(contentType) ? RuntimePolicy.On : RuntimePolicy.Off;
            }
            catch (Exception exception)
            {
                policyContext.Logger.Warn(string.Format(Resources.ExecutePolicyWarning, GetType()), exception);
                return RuntimePolicy.Off;
            }
        }
    }
}