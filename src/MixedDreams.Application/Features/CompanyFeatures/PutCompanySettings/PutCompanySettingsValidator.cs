using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.CompanyFeatures.PutCompanySettings
{
    public class PutCompanySettingsValidator : AbstractValidator<PutCompanySettingsRequest>
    {
        public PutCompanySettingsValidator()
        {
            When(x => x.CooksNumber != null, () => {
                RuleFor(customer => (int)customer.CooksNumber!).GreaterThan(0);
                RuleFor(customer => (int)customer.CooksNumber!).LessThanOrEqualTo(1000);
            });

            When(x => x.MaxSimultaneousOrders != null, () => {
                RuleFor(customer => (int)customer.CooksNumber!).GreaterThan(0);
                RuleFor(customer => (int)customer.CooksNumber!).LessThanOrEqualTo(10000);
            });
        }
    }
}
