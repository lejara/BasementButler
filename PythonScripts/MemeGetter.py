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
    for submission in reddit.subreddit('dankmemes').hot(limit=45):
        if not submission.is_self:
            memeImg.append(submission.url)
    print(memeImg[random.randrange(0, len(memeImg))])


def FourChanSFSMemes(link):
    memePage = requests.get(link)
    soup_shop = BeautifulSoup(memePage.content, "html5lib")

    imgs = {}
    threads = soup_shop.find_all("div", {"class" : "thread"})

    for thread in threads:
        imgLink = "http:" + thread.find("a", {"class" : "fileThumb"})['href']
        messageThread = thread.find("blockquote", {'class' : 'postMessage'}).find(text=True, recursive=False)
        # span = messageThread.find("span") #not working idk, too tired rn
        # if span != None:
        #     messagePost = " ||" + span.text + "||"
        if messageThread == None:
            imgs[imgLink] = ""
        else:
            imgs[imgLink] = " " + messageThread

    randomIndex = random.randrange(0, len(imgs))
    print(list(imgs.keys())[randomIndex] + list(imgs.values())[randomIndex])


def FourChanKeywordSearch(keyword):
    searchPage = requests.get(f'https://find.4chan.org/api?q={keyword}')
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

    randomIndex = random.randrange(0, len(contents))
    print(contents[randomIndex])

def RedditSearchKeyword(keyword):
        reddit = praw.Reddit(client_id='LzijuOQxVi7TRA',
                         client_secret='vKkHdmjobRKjj4K6aNEfzqGwS8U',
                         user_agent='Windows MemeBot by /u/ leoleojar')
        subs = reddit.subreddit('all')
        contents = []
        for sub in subs.search(keyword, limit=65, params={'include_over_18': 'on'}):
            if not sub.is_self:
                splits = sub.url.split('/')
                end = splits[len(splits) - 1]
                if end.find(".")  != -1:
                    contents.append(sub.url)

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
    # RedditSearchKeyword("fun")
    Random_Pick()
