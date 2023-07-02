'use strict'
;(function () {
    let isMobile =
        navigator.userAgent.match(
            /Line|J2ME|Opera Mini|SonyEricsson|Nokia|iPhone|iPod|BB10|PlayBook|android|webOS|BlackBerry|Windows Phone|Windows CE|Android/i
        ) != null
    let scrollTop =
        window.pageYOffset ||
        document.documentElement.scrollTop ||
        document.body.scrollTop
    let carousel = document.querySelector('.carousel')
    let cards = document.querySelectorAll('.card')
    let c1 = document.querySelector('.c1')
    let c2 = document.querySelector('.c2')
    let c3 = document.querySelector('.c3')
    let mouse = document.querySelector('.mouse')
    let status = 0
    let moving = false

    function clickCard() {
        if (!moving) {
            moving = true
            status++
            changeCard(status)
        }
    }
    function changeCard(status) {
        if (status % 3 === 0) {
            cards.forEach((card) => {
                card.classList.remove('move')
                card.classList.remove('stage')
                card.classList.add('active')
            })
            hideCard()
        }
        if (status % 3 === 1) {
            c3.classList.add('move')
            c2.classList.add('stage')
            hideCard(c3)
        }
        if (status % 3 === 2) {
            c2.classList.add('move')
            c1.classList.add('stage')
            hideCard(c2)
        }
    }
    function hideCard(el) {
        if (el) {
            setTimeout(() => {
                el.classList.remove('active')
                el.classList.remove('move')
                el.classList.remove('stage')
            }, 500)
        }
        setTimeout(() => {
            moving = false
        }, 700)
    }
    function cardEnter() {
        mouse.classList.add('hover')
    }
    function cardLeave() {
        mouse.classList.remove('hover')
    }
    function init() {
        if (isMobile) mouse.style.display = 'none'
        window.addEventListener('scroll', function () {
            scrollTop =
                window.pageYOffset ||
                document.documentElement.scrollTop ||
                document.body.scrollTop
        })
        window.addEventListener('mousemove', function (e) {
            mouse.style.top = e.y + scrollTop + 'px'
            mouse.style.left = e.x + 'px'
        })
        carousel.addEventListener('click', clickCard)
        carousel.addEventListener('mouseenter', cardEnter)
        carousel.addEventListener('mouseleave', cardLeave)
        setTimeout(() => {
            cards.forEach((card) => card.classList.add('active'))
        }, 500)
    }
    init()
})()