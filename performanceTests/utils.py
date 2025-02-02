from datetime import datetime
import random
import string

import requests

def generate_random_string(length):
    letters = string.ascii_letters  # contains 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'
    return ''.join(random.choice(letters) for _ in range(length))

def get_datetime_now_string():
    current_timestamp = datetime.now()
    return current_timestamp.strftime('%Y-%m-%d %H:%M:%S')

def wait_until_user_confirms(expected_string):
    while True:
        user_input = input(f"Type '{expected_string}' to start tests: ")
        if user_input.lower() == expected_string:
            print(f"You typed '{expected_string}'. Proceeding...")
            break
        else:
            print("Incorrect input, please try again.")

def fetch_data_from_api(url):
    try:
        response = requests.get(url)
        
        if response.status_code == 200:
            return response.json()
        else:
            print(f"Error: {response.status_code}")
            return None
    except requests.exceptions.RequestException as e:
        print(f"An error occurred: {e}")
        return None