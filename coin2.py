def crossprod (list1, list2):
    output = 0
    for i in range(0,len(list1)):
        output += list1[i]*list2[i]

    return output

def breakit(target, coins):
    coinslimit = [int(target / coins[i]) for i in range(0,len(coins))]
    count = 0
    temp = []
    for i in range(0,len(coins)):
        temp.append([j for j in range(0,coinslimit[i]+1)])


    r=[[]]
    for x in temp:
        t = []
        for y in x:
            for i in r:
                t.append(i+[y])
        r = t

    for targets in r:
        if crossprod(targets, coins) == target:
            print(targets)
            count +=1
    return count




if __name__ == "__main__":
    coins = [5, 3, 2]
    target = 2*2
    print('2x2')
    print(breakit(2*2, coins))
    print('3x3')
    print(breakit(3*3, coins))
    print('4x4')
    print(breakit(4*4, coins))
    print('5x5')
    print(breakit(5*5, coins))


