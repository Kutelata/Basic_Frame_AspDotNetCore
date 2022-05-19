#!/bin/bash
function build(){
    echo "-------------Start-----------------"
    export ASPNETCORE_ENVIRONMENT=$1
    echo "Install modules"
    npm i -f
    echo "Build Front End"
    npm run $3
    echo "Build Back End"
    dotnet publish -r linux-x64 ./Loan2022.$2/ --output output
    echo "-------------Done------------------"
}
