# Performance tests

Tests are written with LOCUST load testing tool <https://locust.io/>

## Requirements

- docker
- python
- locust (via pip)

## Before running performance tests

1. Change variables
   - ENVIRONMENT=Production
3. Run BE services

### Run tests with UI

1. Open script ./basket_requests.py. Adjust variable values if needed. Take care about:
   - USERS_COUNT
   - specify_logging_level()
2. Run locust -f ./basket_requests.py  --csv stats/basket_requests  --run-time 1h
3. Open <http://localhost:8089/> and set users count to the same value as USERS_COUNT in the script
4. Statistics will be present in folder ./stats

### Run tests headless

1. Open script ./basket_requests.py. Adjust variable values if needed. Take care about:
   - USERS_COUNT
   - specify_logging_level()
2. Run locust -f ./basket_requests.py --headless -u 50 -r 1 --csv stats/basket_requests --run-time 1h. Value after -u must match USERS_COUNT from the script
3. Statistics will be present in folder ./stats

### Run tests without active terminal

1. Open script ./basket_requests.py. Adjust variable values if needed. Take care about:
   - USERS_COUNT
   - specify_logging_level()
2. Open script ./run.sh. Adjust users count (argument -u) and run-time
3. Make it executable: chmod +x run.sh
4. Run ./run.sh
5. Statistics will be present in folder ./stats
6. (Optional) See if tests are running: ps aux | grep basket_requests.py

## Visualize results

1. Make sure file basket_requests_stats_history.csv exists in ./stats
2. Run ./visualize.py
