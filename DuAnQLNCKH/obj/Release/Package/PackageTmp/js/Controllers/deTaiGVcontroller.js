var detai = {
    init: function () {
        detai.regEvents();
    },
    regEvents: function () {
     

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('maDeTai') },
                url: '/DeTaiGV/xoaDeTai',
                dataType: 'ActionResult',
                type: 'Delete',
                success: function (res) {
                    if (res.status == true) {
                        alert("Xóa thành công");
                    }
                }
            })
        });
    }
}

detai.init();