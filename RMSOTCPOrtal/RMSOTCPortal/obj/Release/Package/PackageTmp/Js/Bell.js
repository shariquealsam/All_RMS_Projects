function Bell() {
    // alert("Bell");
    //var notification=0;
    $.ajax({
        url: 'Home/CountMessageNotification',
        data: {},
        dataType: "json",
        type: "POST",
        traditional: true,
        success: function (data) {
            notification = data;
            //alert(data);
            var el = document.querySelector('.notification');

            //var count = Number(el.getAttribute('data-count')) || 0;
            var count = Number(el.getAttribute('data-count')) || 0;
            el.setAttribute('data-count', notification);
            el.classList.remove('notify');
            el.offsetWidth = el.offsetWidth;
            el.classList.add('notify');
            if (count === 0) {
                el.classList.add('show-count');
            }
            //if (notification > 0) {
            //    el.classList.add('show-count');
            //    //var audio = new Audio('http://localhost:4686/Image/bell_ring.mp3');
            //    //audio.play();

            //}
        },
        complete: function () {
        },
        error: function (response) {
            alert(response.responseText);
        },
        failure: function (response) {
            alert(response.responseText);
        }
    })
}