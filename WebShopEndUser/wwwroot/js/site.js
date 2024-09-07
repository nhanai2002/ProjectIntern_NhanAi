function DisplayGeneralNotification(message, title) {
    setTimeout(function () {
        toastr.options = {
            closeButton: true,
            progressBar: true,
            showMethod: 'slideDown',
            timeOut: 4000
        };
        toastr.info(message, title);

    }, 1300);
}

function DisplayPersonalNotification(message, title) {
    setTimeout(function () {
        toastr.options = {
            closeButton: true,
            progressBar: true,
            showMethod: 'slideDown',
            timeOut: 4000
        };
        toastr.success(message, title);

    }, 1300);
}

function showNotification() {
    let notify = document.querySelectorAll(".header__notify");
    notify.forEach(function (item) {
        item.style.display = (item.style.display === "block") ? "none" : "block";
    });

}

function seenNotification(element) {
    element.classList.remove("header__notify-item--viewed");
}



// lấy thông báo cũ
function getNotification() {
    let notify = document.querySelectorAll(".header__notify-list");
    let userId = document.getElementById("hfUserId").value;
    $.ajax({
        data: {
            userId: userId
        },
        type: "POST",
        dataType: "json",
        url: '/Home/GetAllNotiByUser',
        cache: false,
        success: function (data) {
            renderNotification(notify, data);
        },
        error: function () {
        },
        complete: function () {
        }
    });

}

// đẩy ra view
function renderNotification(notifyElement, data) {
    var parsedData = JSON.parse(JSON.stringify(data));
    notifyElement.forEach(function (item) {
        if (parsedData && parsedData.length > 0) {
            parsedData.forEach(function (noti) {
                var listItem = `
                        <li class="header__notify-item header__notify-item--viewed">
                            <a href="#" class="header__notify-link">
                                <img src="/assets/image/no-login.png" class="header__notify-img" />
                                <div class="header__notify-info">
                                    <span class="header__notify-name">${noti.title}</span>
                                    <span class="header__notify-description">${noti.message}</span>
                                    <span class="header__notify-time">${noti.sendAt}</span>
                                </div>
                            </a>
                        </li>`;
                item.innerHTML += listItem;
            });
        }
        else {
            // Nếu không có thông báo nào
            var noNoti = `
                        <div class="header__notify-no-login">
                            <img src="/assets/image/no-login.png" alt="" class="header__notify-img">
                            <div class="header__notify-lable">Chưa có thông báo</div>
                        </div>
                        `;
            item.innerHTML += noNoti;
        }
    });

}

getNotification();

// thông báo vừa mới gửi
function addNotification(data) {
    let list = document.querySelectorAll(".header__notify-list");
    let item = document.createElement('li');
    var parsedData = JSON.parse(data);
    item.className = "header__notify-item header__notify-item--viewed";
    item.innerHTML = `
        <a href="#" class="header__notify-link">
            <img src="/assets/image/no-login.png" class="header__notify-img" />
            <div class="header__notify-info">
                <span class="header__notify-name">${parsedData.Title}</span>
                <span class="header__notify-description">${parsedData.Message}</span>
                <span class="header__notify-time">${parsedData.SendAt}</span>
            </div>
        </a>
    `;
    list[0].prepend(item); // Thêm vào đầu danh sách
    //window.location.reload();
}