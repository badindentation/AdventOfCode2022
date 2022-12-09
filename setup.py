import os
import datetime
import urllib.request
import requests

input_url = "https://adventofcode.com/{year}/day/{day}/input"

# Contains the session cookie. (Open advent of code page in dev tools)
session = open("session.txt", 'r').read().strip()

def main(argc, argv):
    # print("Usage: python3 setup.py <day> <year>")

    # Get the current datetime
    today = datetime.datetime.now()
    url = input_url.format(day=today.day if argc < 1 else int(argv[0]), year=today.year)
    # print(url)
    jar = requests.cookies.RequestsCookieJar()
    jar.set('session', session)

    response = requests.get(url, cookies=jar)

    current_directory = os.getcwd()
    day_str = "Day" + str(today.day)
    day_directory = os.path.join(current_directory, day_str) 

    if not os.path.exists(day_directory):
        os.mkdir(day_directory)
        print("Created directory: " + day_directory)
    else:
        print("Directory already exists: " + day_directory)

    input_file = os.path.join(day_directory, "input.txt")
    if not os.path.exists(input_file):
        with open(input_file, 'w') as f:
            f.write(response.text)
        print("- Created file: " + input_file)

    with open("Template/Template.cs", 'r') as f:
        template = f.read().replace("Template", day_str)
        file_name = os.path.join(day_directory, day_str + ".cs")
        with open(os.path.join(day_directory, file_name), 'w') as f:
            f.write(template)
        print("- Created file: " + file_name)
            

if __name__ == "__main__":
    import sys
    args = sys.argv[1:]
    main(len(args), args)

