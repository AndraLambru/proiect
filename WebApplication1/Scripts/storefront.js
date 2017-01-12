var defaultImg = "/Content/img/mountains.png";
var imgTargetid = "";
var flagAddPozaMica = "";
var newIdImg = "";

var IMAGES_CONTENT = new FormData(); // filled dynanically with contents related to images to be uploaded

$(document).ready(function () {
    // When the user clicks the button, open the modal
    EnsureBehaviour();

    $('#btnAddProduct').click(function () {
        cleanmodal();
        $('#product-add-modal').modal();
    });


    $("#btnAddNewProduct").click(function () {
        //verificare();        
        if (verificare() == true) {
            if (saveProduct() == true) {
                
                addProduct();
                $('#product-add-modal').modal('hide');
                EnsureBehaviour();
            }
        }
    });
});


function EnsureBehaviour() {

    $('.open-product').click(function () {
        cleanPreviewModal();
        populateModal(this.id);
        EnsureBehavior2();
        $('#product-detail-modal').modal();
    });

    EnsureBehavior2();
    setNoProducts();
}

function EnsureBehavior2() {

    $(".bagImage").click(function () {
        //aici trebuie sa schimb adresa
        $(".bigBagImage").attr("src", $(this).attr("src"));
    });
}

//
// Populate the "see more.." modal
//
function populateModal(id) {
    id = id.substring(14);
    var pret = "";
    var titlu = "";
    var desc = "";


    $(".col-lg-6").each(function () {
        var idcurent = $(this).attr("infoprod");
        
        if (idcurent != null && idcurent == id) {
            var current = $(this);
           
            pret = $(this).find(".infopret").text();
            titlu = $(this).find(".titluGeanta").text();
            desc = $(this).find(".descGeanta").text();

            $(current).find("input[type=hidden]").each(function () {
                var c = $(this).attr("class").replace("image", "poza");
                var t = "<div class='col-lg-12'>";
                var id = getId();
                t = t + "<img src='' class='img-responsive left_popimages " + c + " bagImage' id='" + id + "'/>";
                t = t + "</div>";
                $(".3pictures").append(t);
                $("#" + id).attr("src", $(this).val());
            });


            $('#product-image-big').attr("src", $(this).find("img").attr('src'));
        }
    })
    $('.pretprod').html(pret);
    $('.titluprod').html(titlu);
    $('.descprod').html(desc);
}

$(document).ready(function () {
    $("#idFile").change(function () {
        var selectedFile = $(this).val();
        //$(".addbigBagImage").attr("src",selectedFile);
        id = readURL(imgTargetid);
    });

    $(".addpozaMare").click(function () {
        imgTargetid = "#addbigBagImage";
        flagAddPozaMica = "";
        $("#idFile").click();
    });

    $(".addpoza").click(function () {
        imgTargetid = "#addpoza";
        flagAddPozaMica = "1";
        $("#idFile").click();
    });
});

function verificare() {

    if ($('#addTitle').val() == '') {
        $('#messageError').text("You must give a title to your product.");
        $('.alert-danger').show();
        return false;
    }
    else
        if ($('#addPrice').val() == '') {
            $('#messageError').text("You must give a price to your product.");
            $('.alert-danger').show();
            return false;
        }
        else
            if (isNaN(parseFloat($('#addPrice').val()))) {
                $('#messageError').text("Invalid Price.");
                $('.alert-danger').show();
                return false;
            }
            else
                if ($('#addDesc').val() == '') {
                    $('#messageError').text("Please add a description of your product.");
                    $('.alert-danger').show();
                    return false;
                }
                else
                    if ($('#addbigBagImage').attr("src") == null || $('#addbigBagImage').attr("src") == defaultImg) {
                        $('#messageError').text("You must add an image of your product.");
                        $('.alert-danger').show();
                        return false;
                    }

    return true;
}

function saveProduct()
{
    var res = true;
    var id = 0;
    var newDescription = $('#addDesc').val();

    var newName = $('#addTitle').val();

    var newPrice = $('#addPrice').val();

    $.ajax({
        type: "POST",
        url: "/Home/SaveBag",
        data:
            {
                name: newName,
                description: newDescription,
                price: newPrice
            }
    })
    .done(function (data) {
        
        if (data != null && data.result != "") {
            alert("null...." + data.result);
            res = false;
        }
        
        id = parseInt(data.id);
        //alert("id="+id);
        if (id > 0) {
            // save "main" image...................................................................................
            var maindata = new FormData();            
            maindata.append("picture", IMAGES_CONTENT.get("addbigBagImage"));
            maindata.append("id_bag", id);
            maindata.append("guid", getId()); // need to save it with a unique name, while [addbigBagImage] is only for'local' use
            maindata.append("rank", 0);
            
            $.ajax({
                type: "POST",
                url: "/Home/SavePic",
                data: maindata,
                dataType: 'json',
                processData: false,
                contentType: false,
                mimeType: 'multipart/form-data'
            })
            .fail(function (data) {
                //alert("fail adding 'main' images");
            });

            // save "left" images.....................................................................................
            var i = 0;
            $(".added img").each(function () {
                if (!($(this).hasClass("trash"))) {
                    i++;
                    var idImg = $(this).attr("id");

                    var data = new FormData();
                    data.append("picture", IMAGES_CONTENT.get($(this).attr("id")));
                    data.append("id_bag", id);
                    data.append("guid", idImg);
                    data.append("rank", i);
                    

                    $.ajax({
                        type: "POST",
                        url: "/Home/SavePic",
                        data: data,
                        dataType:'json',
                        processData: false,
                        contentType: false,
                        mimeType: 'multipart/form-data'
                    })
                    .done(function (data) {
                       // console...

                    })
                    .fail(function (data) {
                        //alert("fail adding 'small' images");
                    });

                }
            });
        }
        
    })
    .fail(function (data) {
        res = false;
    });
    return res;
}


function addProduct() {
    var newInfoProd = getNextId();

    var newText = $('#addDesc').val();

    var newTitle = $('#addTitle').val();

    var newPrice = $('#addPrice').val();

    var template = buildTemplate(newInfoProd, newText, newTitle, newPrice);
    $(".containerProducts").append(template);

    $("#" + newIdImg).attr("src", $("#addbigBagImage").attr("src"));
}

function nbProducts() {
    var k = 0;
    $(".col-lg-6").each(function () {
        if ($(this).attr("infoprod") != null)
            k++;
    });
    return k;
}

function setNoProducts() {
    $("#noProd").text("You have " + nbProducts() + " products");
}

function buildTemplate(newInfoProd, newText, newTitle, newPrice) {
    newIdImg = getId();
    var newDescription = newText.substring(0, 30);
    var t = "<div class='col-lg-6' infoprod='" + newInfoProd + "'>";
    t = t + "<div class='col-lg-12 styleImages' style='background-color: white'>" +
       "<img class='img-responsive' id='" + newIdImg + "'/> ";
    t = t + "<div class='col-lg-8 coloana_scris'>";
    t = t + "<h4 class='titluGeanta'>" + newTitle + "</h4>";
    t = t + "<h5 >" + newDescription + "</h5>";
    t = t + "<p class='descGeanta' style='display:none'>" + newText + "</p>";
    t = t + "<button id='openproductbtn" + newInfoProd + "' class='classWhite open-product blue-text'>see more...</button>";
    t = t + "</div>";
    t = t + "<div class='col-lg-4 pull-right' style='margin-right:-15px'>";
    t = t + "<h3 class='pull-right infopret' style='color: #008000'>" + newPrice + "RON</h3>";
    t = t + "</div>";
    t = t + "<div class='classBuy'>";
    t = t + "<button class='btn btn-primary col-lg-12 classBuy'>Buy</button>";
    t = t + "</div>";

    //t=t+"<input type='hidden' class='image1' value=''>";
    var i = 0;
    $(".added img").each(function () {
        if (!($(this).hasClass("trash"))) {
            i++;
            t = t + "<input type='hidden' class='image" + i + "' value='" + $(this).attr("src") + "'>";
        }
    });

    t = t + "</div>";
    t = t + "</div>";
    return t;
}

//
// Load local image
//
function readURL(targetid) {
    if ($("#idFile").val()) {
        var idLocal = getId();

        var reader = new FileReader();
        reader.onload = function (e) {
            if (flagAddPozaMica == "") {
                $(targetid).attr('src', reader.result);
                idLocal = "addbigBagImage";
            }
            else {
                
                //var template="<img id='"+idLocal+"' src='mountains.png' class='img-responsive left_popimages'/>";
                var template = "<div id='div" + idLocal + "' class='pozaMicaContainer'>" +
                    "<img id='" + idLocal + "' src='mountains.png' class='img-responsive left_popimages'/>" +
                    "<img src='/Content/img/trash.png' class='img-responsive left_popimages trash'  onclick='javascript: $(this).parent().remove();'/>" +
                    "</div>";

                $(".added").append(template);
                $("#" + idLocal).attr('src', reader.result);
                $("#" + idLocal).attr('bcontent', reader.result);                
            }
        }

        reader.readAsDataURL(idFile.files[0]);

        try {
            if (flagAddPozaMica == "") {
                IMAGES_CONTENT.append("addbigBagImage", idFile.files[0]);
            }
            else {
                IMAGES_CONTENT.append(idLocal, idFile.files[0]);
            }
        }
        catch (e) {
            alert(e.description);
        }
        
    }
}

//
// Clean modals
//
// "see more"
function cleanPreviewModal() {

    $('.3pictures').html("");
    $('#product-image-big').attr("src", "");
    $('#product-desc').find("h4").text("");
    $('#product-desc').find("p").text("");
    $("#price").text("");
}
// "add product"
function cleanmodal() {
    $('#addTitle').val("");//null
    $('#addPrice').val("");
    $('#addDesc').val("");

    $('#addbigBagImage').attr('src', defaultImg);
    $('.added').html("");//pt div

    $('.alert-danger').hide();

    IMAGES_CONTENT = new FormData(); // clean content
}
//
// Helpers
//
function getNextId() {
    var newInfoProd = 0;
    $(".col-lg-6").each(function () {
        var idcurent = $(this).attr("infoprod");
        if (idcurent != null) {
            var oldInfoProd = parseInt(idcurent);
            if (newInfoProd < oldInfoProd)
                newInfoProd = oldInfoProd;
        }

        newInfoProd++;

    });
    return newInfoProd;
}

//
// Generate GUID
// Ref.: http://guid.us/GUID/JavaScript
//
function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}
function getId() {
    return guid = (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
}
