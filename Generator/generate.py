import os
import random

def generate_random(poly):
	#parse
	test = poly.split('+')[0]
	ammount = int(test.replace('x',''))
	to_return = ""
	for i in range(0, ammount-1):
		if random.randint(0,10) > 5:
			to_return += '0'
		else :
			to_return += '1'
	to_return += '1'
	
	list_t = list(to_return)
	for i in range(0,20):
		random.shuffle(list_t)
	return ''.join(list_t)

cnt = 1
file = open("polynomals.txt", 'r')
data = file.readlines()
os.makedirs('randomInit')
for i in range(0,len(data)):
	for j in range(i+1, len(data)-2):
		for k in range(j+1, len(data)):
			os.makedirs('randomInit/' + str(cnt))
			tmp = open('randomInit/' + str(cnt) + '/poly1.txt', 'w')
			tmp.write(data[i].replace('\n',''))
			tmp.write('\n' + generate_random(data[i]))
			tmp.close()
			tmp1 = open('randomInit/' + str(cnt) + '/poly2.txt', 'w')
			tmp1.write(data[j].replace('\n',''))
			tmp1.write('\n' + generate_random(data[j]))
			tmp1.close()
			tmp2 = open('randomInit/' + str(cnt) +'/poly3.txt', 'w')
			tmp2.write(data[k].replace('\n',''))
			tmp2.write('\n' + generate_random(data[k]))
			tmp2.close()
			cnt = cnt + 1
			