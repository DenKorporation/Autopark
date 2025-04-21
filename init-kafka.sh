#!/bin/bash

TOPICS=("user-replication")
for TOPIC in "${TOPICS[@]}"
do
  kafka-topics --create \
    --bootstrap-server kafka:9092 \
    --replication-factor 1 \
    --partitions 2 \
    --topic "$TOPIC" \
    --if-not-exists
done