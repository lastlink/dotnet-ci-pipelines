#!/bin/bash
echo ':::start sonar_before_script.sh:::'
apt-get update > /dev/null
#  && apt-get install -qq -y openjdk-16-jdk > /dev/null
# find new links on https://jdk.java.net/22/
wget -q https://download.java.net/java/GA/jdk22.0.2/c9ecb94cd31b495da20a27d4581645e8/9/GPL/openjdk-22.0.2_linux-x64_bin.tar.gz
tar -xvf openjdk-22.0.2_linux-x64_bin.tar.gz -C $HOME > /dev/null 2>&1
echo 'export JAVA_HOME=$HOME/jdk-22.0.2' >> ~/.bashrc
echo 'export PATH=$JAVA_HOME/bin:$PATH' >> ~/.bashrc
source ~/.bashrc
java -version
echo | dotnet --version
dotnet tool install dotnet-sonarscanner --tool-path tools --version 5.5.3
cp SonarQube.Analysis.xml tools/.store/dotnet-sonarscanner/5.5.3/dotnet-sonarscanner/5.5.3/tools/net5.0/any/SonarQube.Analysis.xml
echo ':::end sonar_before_script.sh:::'