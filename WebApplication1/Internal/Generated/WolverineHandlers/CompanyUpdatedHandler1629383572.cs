// <auto-generated/>
#pragma warning disable

namespace Internal.Generated.WolverineHandlers
{
    // START: CompanyUpdatedHandler1629383572
    public class CompanyUpdatedHandler1629383572 : Wolverine.Runtime.Handlers.MessageHandler
    {


        public override async System.Threading.Tasks.Task HandleAsync(Wolverine.Runtime.MessageContext context, System.Threading.CancellationToken cancellation)
        {
            // The actual message body
            var companyUpdated = (WebApplication1.Companies.CompanyUpdated)context.Envelope.Message;

            
            // The actual message execution
            await WebApplication1.Companies.CompanyUpdatedHandler.Handle(companyUpdated).ConfigureAwait(false);

        }

    }

    // END: CompanyUpdatedHandler1629383572
    
    
}
