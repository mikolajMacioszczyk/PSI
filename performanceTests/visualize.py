import pandas as pd
import matplotlib.pyplot as plt

file_path = 'stats/basket_requests_stats_history.csv'
data = pd.read_csv(file_path)

cols_of_interest = ['Timestamp', 'Requests/s', 'Failures/s', '50%', '90%', '95%', '99%', 'Total Average Response Time', 'Total Median Response Time', 'Total Max Response Time']
data = data[cols_of_interest]

data['Timestamp'] = pd.to_datetime(data['Timestamp'], unit='s')
data.set_index('Timestamp', inplace=True)

# Requests/s and Failures/s (count)
plt.subplot(3, 1, 1)
plt.plot(data.index, data['Requests/s'], label='Requests/s', color='blue')
plt.plot(data.index, data['Failures/s'], label='Failures/s', color='red')
plt.title('Requests and Failures Over Time')
plt.xlabel('Timestamp')
plt.ylabel('Requests/Failures per second')
plt.legend(loc='upper right')

# plot 50%, 90%, 95% and 99% Percentile (response time)
plt.subplot(3, 1, 2)
plt.plot(data.index, data['50%'], label='50th Percentile Response Time', color='cyan')
plt.plot(data.index, data['90%'], label='90th Percentile Response Time', color='orange')
plt.plot(data.index, data['95%'], label='95th Percentile Response Time', color='green')
plt.plot(data.index, data['99%'], label='99th Percentile Response Time', color='purple')
plt.title('Percentile Response Times (50%, 90%, 95%, 99%)')
plt.xlabel('Timestamp')
plt.ylabel('Response Time (ms)')
plt.legend(loc='upper right')

# plot Total Average, Median and Max Response Time
plt.subplot(3, 1, 3)
plt.plot(data.index, data['Total Average Response Time'], label='Total Average Response Time', color='purple')
plt.plot(data.index, data['Total Median Response Time'], label='Total Median Response Time', color='orange')
plt.plot(data.index, data['Total Max Response Time'], label='Total Max Response Time', color='brown')
plt.title('Response Times (Average, Median, Max)')
plt.xlabel('Timestamp')
plt.ylabel('Response Time (ms)')
plt.legend(loc='upper right')

plt.tight_layout()
plt.show()