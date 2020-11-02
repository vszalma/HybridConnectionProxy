# HybridConnectionProxy

Based on sample code from https://github.com/Azure/azure-relay/tree/master/samples/hybrid-connections/dotnet/simple-http.

I have modidified the following 1 files to allow a Azure relay hybrid connection through a proxy:

1) ...\Client\Program.cs,
2) ...\Server\Program.cs

The code is currently configured to use the system proxy setting of the user running the code. 
There are also lines (currently commented out) to set the proxy URL directly in the code. Search for 'TODO:' to see the changed code.

Also, for this to work, you need to be using dotnet core => 3.0, though I've only tested on 3.1.
