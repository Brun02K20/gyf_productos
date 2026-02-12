#!/bin/bash
set -e

/opt/mssql/bin/sqlservr &
sql_pid=$!

echo "Waiting for SQL Server to be ready..."
ready=""
for i in {1..30}; do
  if /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P "${SA_PASSWORD}" -Q "SELECT 1" > /dev/null 2>&1; then
    ready="yes"
    break
  fi
  sleep 2
done

if [ -z "$ready" ]; then
  echo "SQL Server did not become ready in time."
  exit 1
fi

/opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P "${SA_PASSWORD}" -d master -i /init/init.sql

wait $sql_pid