

const likedIcons = document.querySelectorAll('.vacancies__item__right #liked_ico');
const unlikedIcons = document.querySelectorAll('.vacancies__item__right #unliked_ico');

likedIcons.forEach(likeIcon => {
    likeIcon.addEventListener('click', function(e) {
        e.preventDefault();
        this.style.display = 'none';
        unlikedIcons[Array.from(likedIcons).indexOf(this)].style.display = 'inline-block';
    });
});

unlikedIcons.forEach(unlikeIcon => {
    unlikeIcon.addEventListener('click', function(e) {
        e.preventDefault();
        this.style.display = 'none';
        likedIcons[Array.from(unlikedIcons).indexOf(this)].style.display = 'inline-block ';
    });
});

var tabLinks = document.querySelectorAll('.tab__link');

tabLinks.forEach(function(tabLink) {
    tabLink.addEventListener('click', function() {
        tabLinks.forEach(function(link) {
            link.classList.remove('active');
        });
        tabLink.classList.add('active');

        var tabItemId = tabLink.getAttribute('data-tab');

        var tabItems = document.querySelectorAll('.tab__item');

        tabItems.forEach(function(item) {
            if (item.getAttribute('data-tab') === tabItemId) {
                item.classList.add('active');
            } else {
                item.classList.remove('active');
            }
        });
    });
});

