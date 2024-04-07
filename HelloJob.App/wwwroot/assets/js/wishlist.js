$('.blog-items').slick({
    dots: false,
    infinite: true,
    speed: 300,
    slidesToShow: 3,
    slidesToScroll: 1,
    
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
          infinite: true,
          dots: true
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2
        }
      },
      {
        breakpoint: 560,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          arrows:false


        }
      }
      // You can unslick at a given breakpoint now by adding:
      // settings: "unslick"
      // instead of a settings object
    ]
});


$(document).ready(function () {
$('.resume-like').on('click', '#like', function (e) {
    console.log("clicked in wishlist")
    e.preventDefault();

    var itemId = $(this).closest('.resume-like').data('id');
    var itemType = "resume";

    $.ajax({
        url: '/Resume/AddWishlist',
        type: 'POST',
        dataType: 'json',
        data: {
            itemid: itemId,
            itemtype: itemType
        },
        success: function (response) {
            console.log("Wishlist'a elave olundu");
            location.reload();
        },
        error: function (xhr, status, error) {
            console.error('There has been a problem with your AJAX request:', error);
        }
    });
});

    $('.vacancy-like').on('click', '#like', function (e) {
        console.log("clicked in wishlist")
        e.preventDefault();

        var itemId = $(this).closest('.vacancy-like').data('id');
        var itemType = "vacancy";

        $.ajax({
            url: '/Home/AddWishlist',
            type: 'POST',
            dataType: 'json',
            data: {
                itemid: itemId,
                itemtype: itemType
            },
            success: function (response) {
                console.log("Wishlist'a elave olundu");
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error('There has been a problem with your AJAX request:', error);
            }
        });
    });
   
  const likeIcons = document.querySelectorAll('.card_like');

  likeIcons.forEach(likeIcon => {
      likeIcon.addEventListener('click', function(e) {
          e.preventDefault();
          const wishlistItem = this.closest('.vacancies__item');
          if (wishlistItem) {
              wishlistItem.remove();
          }
      });
  });
});


