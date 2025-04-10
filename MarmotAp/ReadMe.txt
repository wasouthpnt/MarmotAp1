// TO PUBLISH
// In VS2022, Load the project, then open the terminal window (Developer PowerShell) and paste the following line.
// The APK will be the signed one in the bin/Release folder of the project. (MarmotAp.MarmotAp-Signed.apk)
dotnet publish -f net8.0-android -c Release -p:AndroidKeyStore=true -p:AndroidSigningKeyStore=marmotap.keystore -p:AndroidSigningKeyAlias=wyatt -p:AndroidSigningKeyPass=wyattsouth -p:AndroidSigningStorePass=wyattsouth
