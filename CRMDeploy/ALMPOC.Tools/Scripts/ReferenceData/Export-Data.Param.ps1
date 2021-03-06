﻿# Importing common functions
. .\..\common\CrmSolution.Common.ps1
. .\..\common\ReferenceData.Common.ps1

# Defaulting to increased verbosity for manual execution
$oldverbose = $VerbosePreference
$VerbosePreference = "continue"

try
{
	Write-Output "Begin export of configuration data..." 
	
	.\Export-Data.ps1 `
    -serverUrl (Get-CrmDevOrgUrl "ALMPOC.CRM.Schema") `
    -username (Get-CrmUsername "ALMPOC.CRM.Schema") `
    -password (Get-CrmPassword "ALMPOC.CRM.Schema") `
	-dataFilePath "..\..\..\ALMPOC.CRM.Data"

	Write-Output "End export of configuration data..."
}
finally
{
	# Reset the verbosity to original level
	$VerbosePreference = $oldverbose
}