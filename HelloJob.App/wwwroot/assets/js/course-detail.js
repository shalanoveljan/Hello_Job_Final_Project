const likeIcons = document.querySelectorAll('#like');
const unlikeIcons = document.querySelectorAll('#unlike');

likeIcons.forEach(likeIcon => {
    likeIcon.addEventListener('click', function(e) {
        e.preventDefault();
        this.style.display = 'none';
        unlikeIcons[Array.from(likeIcons).indexOf(this)].style.display = 'inline-block';
    });
});

unlikeIcons.forEach(unlikeIcon => {
    unlikeIcon.addEventListener('click', function(e) {
        e.preventDefault();
        this.style.display = 'none';
        likeIcons[Array.from(unlikeIcons).indexOf(this)].style.display = 'inline-block';
    });
});