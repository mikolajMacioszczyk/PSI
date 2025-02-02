from enum import Enum

class LogLevel(Enum):
    DEBUG = 1
    WARNING = 2
    NONE = 3

specified_level = LogLevel.DEBUG

def specify_logging_level(log_level):
    global specified_level
    specified_level = log_level

def log(message, message_level):
    if message_level.value >= specified_level.value:
        print(message)
    pass