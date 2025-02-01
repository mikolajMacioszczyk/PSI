#!/bin/bash
nohup python3 -m locust -f ./chat_requests.py --headless -u 50 -r 5 --csv stats/chat_requests --run-time 5h > /dev/null 2>&1 &