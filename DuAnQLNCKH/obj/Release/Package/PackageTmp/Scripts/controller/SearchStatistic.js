

$(document).ready(function () {

    

    function Contains(text_one, text_two) {
        if (text_one.indexOf(text_two) != -1)
            return true;
    }

// statis lecture

    //$("#Search").keyup(function () {
    //    var searchText = $("#Search").val().toLowerCase();
       
    //    $(".Search").each(function () {
    //        var id = $(this).find(".nametd");
           
    //        if (Contains($(id).text().toLowerCase(), searchText)) {
    //            $(this).show();
    //        }
    //        else {
    //            $(this).hide();
    //        }
    //    })
    //})
 
    //$("#StatisticYear").change(function () {
    //    var searchText = $("#StatisticYear").val();

    //    $(".Search").each(function () {
    //        var id = $(this).find(".Year").text().substr(6, 4);

    //        if (!Contains(id, searchText)) {
    //            $(this).hide();
    //        }
    //        else {
    //            $(this).show();
    //        }
    //    })
    //})

    // Tìm kiếm giảng viên

    $("#buttonSearch").click(function () {


        var dateSt = $("#DateSt").val();
        var dateEnd = $("#DateEnd").val();

        var dateEnd1 = new Date(dateEnd);
        var dateSt1 = new Date(dateSt);
       
        var IdTySearch = $("#IdType").val();
        var IdFSearch = $("#IdF").val();
        var IdLeSearch = $("#IdLe").val();
         var list = [];
        $(".Search").each(function () {

            var id = $(this).find(".Year1").val();
            var IdTy = $(this).find(".IdTy").val();
            var IdF = $(this).find(".IdFacu1").val();
            var IdLe = $(this).find(".IdLe1").val();
             //var pointBody = $(this).find(".pointBody");
            if (IdTySearch == "") {
                if (IdFSearch == "") {
                    if (IdLeSearch == "") {
                        
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id)) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                         
                            
                    }
                    else
                    {
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe.match(IdLeSearch)) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                       
                }
                else {
                    if (IdLeSearch == "") {
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdF.match(IdFSearch)) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }


                    }
                    else {
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe.match(IdLeSearch) && IdF.match(IdFSearch)) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                }
            }
            else {
                if (IdFSearch == "") {
                    if (IdLeSearch == "") {                         
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdTy == IdTySearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                          
                    }
                    else {
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe.match(IdLeSearch) && IdTy == IdTySearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                      
                }
                else {
                    if (IdLeSearch == "")
                    {
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdF.match(IdFSearch) && IdTy==IdTySearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                         
                         
                    }
                    else
                    {
                         
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe.includes(IdLeSearch) && IdF == IdFSearch && IdTy.includes(IdTySearch)) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }                                             
                    }
                }
            }
            if ($(this).css('display') != 'none') {
                 
                var objCells = this.cells;
                var itemList = {};
                itemList['NameTopic'] = objCells.item(0).innerText;
                itemList['NameLec'] = objCells.item(1).innerText;
                itemList['Faculty'] = objCells.item(2).innerText;
                itemList['Type'] = objCells.item(3).innerText;
                itemList['DateSt'] = objCells.item(4).innerText;
                itemList['DateEnd'] = objCells.item(5).innerText;
                itemList['Expense'] = objCells.item(6).innerText;
                itemList['Grade'] = objCells.item(7).innerText;
                itemList['Hour'] = objCells.item(8).innerText;
                 
                list.push(itemList);

                   
            }
        })
        
        $.ajax({
            type: "POST",
            url: "/Statistic/ExportExcel1/",            
            data: { list : list },
            dataType: "json",
            success: function () {

            }

        });

    })

   // Tìm kiếm sinh viên

    $("#buttonSearchStu").click(function () {

         
        var dateSt = $("#DateStStu").val();
        var dateEnd = $("#DateEndStu").val();

        var dateEnd1 = new Date(dateEnd);
        var dateSt1 = new Date(dateSt);
         

        //var IdTySearch = $("#IdTyStu").val();
        var IdFSearch = $("#IdFStu").val();
        //var IdLeSearch = $("#IdSV").val();
        //var Pro = $("#ProgressStu").val();
        var list1 = [];
        $(".SearchStu").each(function () {

            var id = $(this).find(".Year1Stu").val();
            //var IdTy = $(this).find(".IdTyStu").val();
            var IdF = $(this).find(".IdFacu1Stu").val();
            //var IdLe = $(this).find(".IdSV1").val();
            if (IdF == "") {
                if (dateSt1 < new Date(id) && dateEnd1 > new Date(id)) {

                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            }
            else {
                if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdF.match(IdFSearch)) {

                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            }
            if ($(this).css('display') != 'none') {
                 
                    var objCells = this.cells;
                    var itemList = {};
                    itemList['NameTopic'] = objCells.item(0).innerText;
                    itemList['NameStu'] = objCells.item(1).innerText;
                    itemList['Faculty'] = objCells.item(2).innerText;
                    itemList['Lec'] = objCells.item(3).innerText;
                    itemList['DateSt'] = objCells.item(4).innerText;
                    itemList['Point'] = objCells.item(5).innerText;

                    list1.push(itemList);
                  
            }
             
        })
        $.ajax({
            type: "POST",
            url: "/Statistic/ExportExcel2/",
            data: { list1 : list1 },
            dataType: "json",
            success: function (data) {

            }

        });

    })

})