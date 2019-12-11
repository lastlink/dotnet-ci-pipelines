# DOTNET CORE CI PIPELINE EXAMPLE: 

Based off of [gitlab-ci-example-dotnetcore](https://gitlab.com/tobiaskoch/gitlab-ci-example-dotnetcore). Goal to have example pipelines for major git source control dev ops including github, azure devops, gitlab, bitbucket, and etc. Please do a pull request to the proper source control pipeline that you are trying to update.


### [ ] Gitlab  [![pipeline status](https://gitlab.com/lastlink/dotnet-ci-pipelines/badges/master/pipeline.svg)](https://gitlab.com/lastlink/dotnet-ci-pipelines/commits/master)  [![coverage status](https://gitlab.com/lastlink/dotnet-ci-pipelines/badges/master/coverage.svg)](https://gitlab.com/lastlink/dotnet-ci-pipelines/commits/master)
* [ ] Badges
    * [x] pipeline
    * [ ] coverage
        * [x] build in
        * [ ] coverlet
            * [] badge_branchcoverage [![badge_branchcoverage](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_branchcoverage.svg)](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_branchcoverage.svg)
            * [] badge_combined [![badge_branchcoverage](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_combined.svg)](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_combined.svg)
            * [] badge_linecoverage [![badge_branchcoverage](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_linecoverage.svg)](https://lastlink.gitlab.io/dotnet-ci-pipelines/badge_linecoverage.svg)
    * [ ] build per job?
* [x] Tests - Junit
* [x] Coverage - coverlet
* [x] Code Quality
    * [x] resharper clis
        * [x] inspectcode 1min
        * [x] dupfinder 1min
    * [x] CodeClimate (complexity & duplication) 10min+ 