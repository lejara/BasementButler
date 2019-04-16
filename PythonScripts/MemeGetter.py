"""
Gets a single image url and print it, may include text if from 4chan random pick.
Sources currently are 4chan and reddit.

Author: Lejara

"""

from bs4 import BeautifulSoup
import requests
import random
import praw
import sys

def GetRedditDankMemes(link):

    reddit = praw.Reddit(client_id='LzijuOQxVi7TRA',
                     client_secret='vKkHdmjobRKjj4K6aNEfzqGwS8U',
                     user_agent='Windows MemeBot by /u/ leoleojar')
    memeImg = []
    for submission in reddit.subreddit('dankmemes').hot(limit=55):
        if not submission.is_self:
            memeImg.append(submission.url)

    print(memeImg[random.randrange(0, len(memeImg))])


def FourChanSFSMemes(link):
    memePage = requests.get(link)
    soup_shop = BeautifulSoup(memePage.content, "html5lib")

    imgs = []
    threads = soup_shop.find_all("div", {"class" : "thread"})

    for thread in threads:
        imgLink = "http:" + thread.find("a", {"class" : "fileThumb"})['href']
        imgs.append(imgLink)

    if len(imgs) != 0:
        print(imgs[random.randrange(0, len(imgs))])


def FourChanKeywordSearch(keyword, current_poll_ammount = 0):
    searchPage = requests.get(f'https://find.4chan.org/api?q={keyword}&o={current_poll_ammount}')
    dict = searchPage.json()
    contents = []
    for thread in dict['threads']:

        post = thread['posts']
        post_dict = post[0]
        if 'ext' in post_dict.keys():
            board = thread['board']
            time = post[0]['tim']
            ext = post[0]['ext']
            sExt = 's' if ext == '.jpg' else ""
            contentLink = f'http://i.4cdn.org/{board}/{time}{sExt}{ext}'
            contents.append(contentLink)

    if len(contents) != 0:
        randomIndex = random.randrange(0, len(contents))
        print(contents[randomIndex])
    else:
        FourChanKeywordSearch(keyword, current_poll_ammount + 10) #recall if nothing is found

def RedditSearchKeyword(keyword):
        reddit = praw.Reddit(client_id='LzijuOQxVi7TRA',
                         client_secret='vKkHdmjobRKjj4K6aNEfzqGwS8U',
                         user_agent='Windows MemeBot by /u/ leoleojar')
        subs = reddit.subreddit('all')
        contents = []
        for sub in subs.search(keyword, limit=80, params={'include_over_18': 'on'}):
            if not sub.is_self:
                splits = sub.url.split('/')
                end = splits[len(splits) - 1]
                if end.find(".")  != -1:
                    contents.append(sub.url)

        if len(contents) != 0:
            print(contents[random.randrange(0, len(contents))])

memeRepoRandom = {
    "No Link": GetRedditDankMemes,
    "http://boards.4chan.org/s4s/": FourChanSFSMemes

}

memeRepoKeyword = [
    FourChanKeywordSearch,
    RedditSearchKeyword
]

def Random_Pick():
    randomIndex = random.randrange(0, len(memeRepoRandom))
    link = list(memeRepoRandom.keys())[randomIndex]
    func = list(memeRepoRandom.values())[randomIndex](link)

def RandomKeyword_Pick(keyword):
    memeRepoKeyword[random.randrange(0, len(memeRepoKeyword))](keyword)


if len(sys.argv) > 1:
    RandomKeyword_Pick(str(sys.argv[1]))
else:
    # RandomKeyword_Pick("fun")
    #FourChanKeywordSearch("fun")
    # RedditSearchKeyword("cookies")
    # FourChanSFSMemes("http://boards.4chan.org/s4s/")
    Random_Pick()
