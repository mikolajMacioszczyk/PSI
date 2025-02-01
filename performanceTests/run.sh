#!/bin/bash
nohup python3 -m locust -f ./basket_requests.py --headless -u 50 -r 5 --csv stats/basket_requests --run-time 5h > /dev/null 2>&1 &