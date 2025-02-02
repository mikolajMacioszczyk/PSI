# Remember to modify keycloak version also below
FROM quay.io/keycloak/keycloak:26.1.0 as builder

WORKDIR /opt/keycloak
RUN /opt/keycloak/bin/kc.sh build

# change Keycloak version also here
FROM quay.io/keycloak/keycloak:26.1.0
COPY --from=builder /opt/keycloak/ /opt/keycloak/
COPY ./imports/Shop_import.Development.json /opt/keycloak/data/import/Shop_import.json

ENTRYPOINT [ "/opt/keycloak/bin/kc.sh", "-v", "start-dev", "--import-realm" ]