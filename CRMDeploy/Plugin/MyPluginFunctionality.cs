using System;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

using Plugin.Helpers;
using Plugin.CrmServices;
using Plugin.Logic;
using Plugin.CrmServices.XrmSdk;


namespace Plugin
{
    /// <summary>
    /// What does this plugin do?
    /// </summary>
    [RegistrationInfo(MessageName = MessageName.Create,
        PrimaryEntityName = Account.EntityLogicalName,
        Stage = MessageProcessingStage.BeforeMainOperationInsideTransaction)]
    [RegistrationInfo(MessageName = MessageName.Update,
        PrimaryEntityName = Account.EntityLogicalName,
        ExecutionOrder = 1,
        Stage = MessageProcessingStage.BeforeMainOperationInsideTransaction,
        PreEntityImageName = "preimage|name,address1_line1",
        FilteringAttributes = "address1_line1")]
	/// Use the PluginRegistration.bat file to automatically register the plugin based 
    public class MyPluginFunctionality : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ManagerBase managerBase = new ManagerBase(serviceProvider, false);

            #region Verify execution context

            managerBase.ValidateExecutionContext(this.GetType(), true);

            #endregion Verify execution context

            try
            {
                // Your code
                Entity target = managerBase.PluginExecutionContext.InputParameters["Target"] as Entity;
                if (target.Contains("address1_line1"))
                {
                    Entity preimage = managerBase.PluginExecutionContext.PreEntityImages["preimage"];
                    string accountName = preimage.GetAttributeValue<string>("name");
                    string accountAddress = preimage.GetAttributeValue<string>("address1_line1");
                    accountName = accountName.Replace(accountAddress, "").TrimEnd();

                    target.Attributes["name"] = accountName + " " + target.GetAttributeValue<string>("address1_line1");
                }

                // Your Application Exception will be translated into the User Language, check the Translations.resx file
				// The plugin will fail and rollback the transaction
                // throw new ApplicationException("EX_NotAuthorized");

				// The plugin will fail and rollback the transaction
                // throw new InvalidPluginExecutionException(OperationStatus.Succeeded,"tis kapot");

				// The plugin will not fail and commit the transaction
				// throw new NonBlockingException("tis kapot", new Exception("tis kapot"));

            }
            catch (Exception ex)
            {
			    // The plugin message will be logged to the Plugin-Trace Log.
				// Use the Recurring Workflow Assembly "HandlePluginTraceLogs" to copy the Plugin-Trace Log message to the "Log"-entity (inf_log)
                ExceptionHandler.HandlePluginException(ex, managerBase, this.GetType().Name);
            }
        }
    }
}
