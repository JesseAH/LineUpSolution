using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LineUp_Website.Models
{
    public interface IBraintreeConfig
    {
        IBraintreeGateway CreateGateway();
        string GetConfigurationSetting(string setting);
        IBraintreeGateway GetGateway();

    }
}