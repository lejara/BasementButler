from bs4 import BeautifulSoup
import requests
import random
import praw

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

memeRepo = {
    "No Link": GetRedditDankMemes,
    "http://boards.4chan.org/s4s/": FourChanSFSMemes

}
randomIndex = random.randrange(0, len(memeRepo))
link = list(memeRepo.keys())[randomIndex]
func = list(memeRepo.values())[randomIndex]

# FourChanSFSMemes("http://boards.4chan.org/s4s/")
func(link)
