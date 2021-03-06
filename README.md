# SharpJDKWrapper
Project to wrap the execution of Java applications within .NET environment. It is useful in all those contexts where there is a need to quickly host and test Java services without using dedicated Application Servers.

### How it works
SharpJDKWrapper is developed in the .NET Framework 4.8 and allows the execution of JARs developed in Java within .NET processes on Windows environment. SharpJDKWrapper can also be installed as a Windows service in order to host JARs for testing purposes, without having to configure Application Servers such as Tomcat.

<img src="SharpJDKWrapper.png" width=50% height=50%>

### How to use it

To use SharpJDKWrapper, you need to configure the following keys within the application config:

```csharp
	<appSettings>
		<add key="JDK_DIRECTORY" value="C:\JDK\bin"/>
		<add key="JAR_DIRECTORY" value="C:\JARs"/>
		<add key="API_ADDRESS" value="http://localhost:12345/"/>
	</appSettings>
```

in the *JDK_DIRECTORY* key, you have to write the directory path where the JDK is installed. 
In the second one *JAR_DIRECTORY*, you have to write the directory path where the JARs are located.
The last one *API_ADDRESS* defines the endpoint on which to expose the APIs.

### API

SharpJDKWrapper exposes some REST APIs to execute some commands, in detail: 

* Retrieve the number of active Java service. 
* Retrieve the state of a specific Java service. 
* Stop a specific Java service.

SharpJDKWrapper integrates **Swagger** at the following address:

http://localhost:12345/swagger

### Future improvements

SharpJDKWrapper can be easily migrated to.NET Core.

### Contributing
Thank you for considering to help out with the source code!
If you'd like to contribute, please fork, fix, commit and send a pull request for the maintainers to review and merge into the main code base.

**Getting started with Git and GitHub**

 * [Setting up Git for Windows and connecting to GitHub](http://help.github.com/win-set-up-git/)
 * [Forking a GitHub repository](http://help.github.com/fork-a-repo/)
 * [The simple guide to GIT guide](http://rogerdudler.github.com/git-guide/)
 * [Open an issue](https://github.com/engineering87/SharpJDKWrapper/issues) if you encounter a bug or have a suggestion for improvements/features

### Licensee
SharpJDKWrapper source code is available under MIT License, see license in the source.

### Contact
Please contact at francesco.delre.87[at]gmail.com for any details.
