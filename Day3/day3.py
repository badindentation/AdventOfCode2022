# Read file input.txt
def priority(c):
    if c >= 'A' and c <= 'Z':
        return ord(c) - ord('A') + 27
    else:
        return ord(c) - ord('a') + 1

with open("input.txt", "r") as f:
    lines = f.read().splitlines()
    sum = 0
    for line in lines:
        first = line[:int(len(line)/2)] 
        second = line[int(len(line)/2):]
        set_1 = set(first)
        set_2 = set(second)
        set_3 = set_1 & set_2

        x = set_3.pop()
        print(f"{first} {second}\n{x} - {priority(x)}")
        sum += priority(x)
    print(sum)
