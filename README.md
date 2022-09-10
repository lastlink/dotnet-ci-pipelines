# DOTNET CORE CI PIPELINE EXAMPLE: 
Goal to have dotnet core c# example pipelines for major git source control dev ops including github, azure devops, gitlab, bitbucket, and etc. Should auto run only on a new pull request, show test results in native ui per website, code coverage, and some additional manual steps running code quality analysis. Badge support native is also nice or from https://shields.io/category/build. Also will try service containers and database unit tests.
Originally off of [gitlab-ci-example-dotnetcore](https://gitlab.com/tobiaskoch/gitlab-ci-example-dotnetcore).  

## Contribution

* Please do a pull request to the proper source control pipeline that you are trying to update.

## TOC
* [GitHub Actions](#--github-actions-)
* [Bitbucket](#--bitbucket)
* [Azure DevOps](#--azure-devops-)
* [Gitlab pipelines](#--gitlab--------)
* [Other notes](#other)

### [ ] [GitHub Actions](https://github.com/lastlink/dotnet-ci-pipelines) [![Actions Status](https://github.com/lastlink/dotnet-ci-pipelines/workflows/.net%20core/badge.svg)](https://github.com/lastlink/dotnet-ci-pipelines/actions)

* [limits](https://help.github.com/en/github/setting-up-and-managing-billing-and-payments-on-github/about-billing-for-github-actions) 2,000 (per month) to 20 concurrent jobs support windows, ubuntu, mac
    * doesn't currently support retry and max timeout although will cancel when limit reached.
* [ ] Badges - workflow
* [x] api docs - docfx html only
* [!] Tests - no display just logs and work flow
* [ ] CodeClimate issues with non npm and docker in docker
* [x] resharper cli - no manual triggers supports
* [x] security dependency scan - snyk
* [x] artifacts 
* [x] [Services](https://docs.github.com/en/actions/configuring-and-managing-workflows/about-service-containers) mysql, [postgres](https://docs.github.com/en/actions/configuring-and-managing-workflows/creating-postgresql-service-containers), and mssql work like a charm
* [] release
  * [] obfuscators

### [ ] [Bitbucket](https://bitbucket.org/lastlink/dotnet-ci-pipelines/src)
* [limits](https://confluence.atlassian.com/bitbucket/limitations-of-bitbucket-pipelines-827106051.html) 50 minutes per month, support docker images
* [ ] Badges
* [x] api docs - docfx html only
* [x]  Tests - working display
    * [x] junit works fine
    * [x] print out code coverage in log, not natively supported
* [!] CodeClimate - docker in docker throwing an error
    * `docker: Error response from daemon: authorization denied by plugin pipelines: -v only supports $BITBUCKET_CLONE_DIR and its subdirectories.`
* [x] resharper cli
* artifacts only last 12 hrs are downloaded in the `.tar.gz` format, could not figure out the curl method
* [x] [Services](https://support.atlassian.com/bitbucket-cloud/docs/use-services-and-databases-in-bitbucket-pipelines/) mysql and postgres work
  * mssql starts up, gives lots of details on docker image running, not enough detail for why test was failing

### [ ] [Azure DevOps](https://dev.azure.com/funktechno/dotnet%20ci%20pipelines) [![Build Status](https://dev.azure.com/funktechno/dotnet%20ci%20pipelines/_apis/build/status/dotnet%20ci%20pipelines?branchName=master)](https://dev.azure.com/funktechno/dotnet%20ci%20pipelines/_build/latest?definitionId=1&branchName=master)
* [limits](https://azure.microsoft.com/en-us/services/devops/pipelines/) 1,800 minutes per month on private projects. $40 a month for unlimited minutes.
* [ ] Badges?
  * [x] build
* [x] api docs - docfx html only (pdf works locally, maybe self hosted runner)
* [x] Tests - working display
* [x] CodeClimate 3m
* [x] resharper cli
* [x] artifacts
* [x] [Service Containers](https://docs.microsoft.com/en-us/azure/devops/pipelines/process/service-containers?view=azure-devops&tabs=yaml) mysql and postgress work fine
  * mssql fails to connect after container should be initialized successfully. (only initializes for linux agents)
  * really want mssql to work for ease w/ some integrated testing and pre deploy checks. tracking progress in [stackoverflow](https://stackoverflow.com/questions/63538477/how-do-i-get-mssql-service-container-working-in-azure-devops-pipeline) and [developercommunity.visualstudio.com](https://developercommunity.visualstudio.com/content/problem/1159426/working-examples-using-service-container-of-sql-se.html)

### [ ] [Gitlab](https://gitlab.com/lastlink/dotnet-ci-pipelines) [![pipeline status](https://gitlab.com/lastlink/dotnet-ci-pipelines/badges/master/pipeline.svg)](https://gitlab.com/lastlink/dotnet-ci-pipelines/commits/master)  [![coverage status](https://gitlab.com/lastlink/dotnet-ci-pipelines/badges/master/coverage.svg)](https://gitlab.com/lastlink/dotnet-ci-pipelines/commits/master) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=lastlink_dotnet-ci-pipelines&metric=alert_status)](https://sonarcloud.io/dashboard?id=lastlink_dotnet-ci-pipelines) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=lastlink_dotnet-ci-pipelines&metric=code_smells)](https://sonarcloud.io/dashboard?id=lastlink_dotnet-ci-pipelines) [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=lastlink_dotnet-ci-pipelines&metric=sqale_index)](https://sonarcloud.io/dashboard?id=lastlink_dotnet-ci-pipelines) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=lastlink_dotnet-ci-pipelines&metric=coverage)](https://sonarcloud.io/dashboard?id=lastlink_dotnet-ci-pipelines) [![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=lastlink_dotnet-ci-pipelines&metric=ncloc)](https://sonarcloud.io/dashboard?id=lastlink_dotnet-ci-pipelines)
* [limits](https://about.gitlab.com/pricing/) 400 ~~2,000~~ minutes per month per group or user. $10 to buy 1,000 extra minutes expires end of year.
* [ ] Badges
    * [x] pipeline
    * [ ] coverage
        * [x] build in
        * [ ] coverlet
            * [] badge_branchcoverage [![badge_branchcoverage](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_branchcoverage.svg)](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_branchcoverage.svg)
            * [] badge_combined [![badge_branchcoverage](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_combined.svg)](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_combined.svg)
            * [] badge_linecoverage [![badge_branchcoverage](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_linecoverage.svg)](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_linecoverage.svg)
    * [ ] build per job?
* [x] api docs - docfx
* [x] Tests - Junit
* [x] Coverage - coverlet
* [x] [Services](https://docs.gitlab.com/ee/ci/docker/using_docker_images.html#what-is-a-service) - [mysql](https://docs.gitlab.com/ee/ci/services/mysql.html), postgres, mssql all work
  * mssql occasionally fails to connect and step just needs to be rerun.
* [x] Code Quality
    * [x] resharper clis
        * [x] inspectcode 1min
        * [x] dupfinder 1min
    * [x] CodeClimate (complexity & duplication) 10min+ 
* [x] artifacts

### [Google Source Repository](https://source.cloud.google.com/onboarding/welcome)
* [pricing](https://cloud.google.com/source-repositories/pricing#pricing) free up to 5 users. [limits](https://cloud.google.com/build/pricing) first 120 minutes a day free. $30 per 1000 priced per minute. 
* [] Tests - Junit TODO

## Other

### Database
* `dotnet ef migrations add InitialMigrations --project ../MyProject.Repository/MyProject.Repository.csproj --startup-project ./MyProject.Api.csproj`
* `dotnet ef migrations remove`
* `dotnet ef database update --project ../MyProject.Repository/MyProject.Repository.csproj --startup-project ./MyProject.Api.csproj`