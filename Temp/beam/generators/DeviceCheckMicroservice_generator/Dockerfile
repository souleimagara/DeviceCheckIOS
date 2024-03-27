
FROM beamableinc/beamservice:prod_1.19.15 AS build-env
RUN dotnet --version
WORKDIR /subapp


COPY devicecheckmicroservice_generator.csproj .

RUN echo $BEAMABLE_SDK_VERSION > /subapp/.beamablesdkversion



EXPOSE 6565
ENV BEAMABLE_SDK_VERSION_EXECUTION=1.19.15
ENV DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1
RUN dotnet restore .
ENTRYPOINT ["dotnet", "watch"]
