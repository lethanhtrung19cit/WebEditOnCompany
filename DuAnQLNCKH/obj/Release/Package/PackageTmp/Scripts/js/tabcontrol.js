    $(document).ready(function () {
        $(".tab-nav-wrapper li.active").click();
        $($(".tab-nav-wrapper li.active a").attr("href")).show();

        $(".tab-nav-wrapper li").click(function (e) {
        e.preventDefault();
            e.stopPropagation();

            $(".tab-nav-wrapper li").removeClass("active");
            $(this).addClass("active");

            var target = $(this).find("a").attr("href");
            $(".tab-content-wrapper").find(".tab-content").hide();
            $(".tab-content-wrapper").find(target).show();
        })
    });
