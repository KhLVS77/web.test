﻿function createCookie(name, value, days) {
    var expires;
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    }
    else {
        expires = "";
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}

function deleteCookie(name) {
    var value = getCookie(name);

    if (value) {
        createCookie(name, value, -100);
    }
}

function getCookie(name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(name + "=");
        if (c_start != -1) {
            c_start = c_start + name.length + 1;
            c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1) {
                c_end = document.cookie.length;
            }
            return unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return "";
}

async function Sleep(ms) {
    await new Promise((resolve, reject) => setTimeout(resolve, ms));
}

function History() {
    window.location = 'History';
}

function Settings() {
    window.location = 'Settings';
}

function Calculator() {
    window.location = 'Calculator';
}

function Register() {
    window.location = '/Register';
}

function Logout() {
    const response = confirm("Are you sure you want to logout?");

    if (response) {
        deleteCookie('login');
        window.location = '/';
    }
}

function verifyLoggedIn() {
    var baseUrl = getBaseUrl();
    var name = getCookie('login');
    var noLogin = [
        baseUrl,
        baseUrl + '/',
        baseUrl + '/Register'
    ];

    if (!name && !noLogin.includes(window.location.href)) {
        Logout();
    }
}

function getBaseUrl() {
    var getUrl = window.location;
    return getUrl.protocol + "//" + getUrl.host;
}

async function SetDropdownValuesFromValues(id, selected) {
    $.ajax({
        type: 'GET',
        url: 'api/settings/values',
        dataType: 'json',
        data: { 'name': id },
        success: async function (response) {
            await SetDropdownValues(id, response, selected);
        }
    });
}

async function SetDropdownValues(id, values, selected) {
    var dropdown = document.getElementById(id);

    for (const value of values) {
        var option = document.createElement("option");
        option.value = value;
        option.text = value;
        dropdown.appendChild(option);
    }

    dropdown.selectedIndex = selected;
    return;
}

async function SetDropdownSelectedValue(id, value) {
    var dropdown = document.getElementById(id);

    for (var i = 0; i < dropdown.options.length; i++) {
        if (dropdown.options[i].text == value) {
            dropdown.options[i].selected = true;
            return;
        }
    }
}

verifyLoggedIn();
