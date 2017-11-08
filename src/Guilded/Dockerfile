FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 80 443
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Guilded.dll"]
