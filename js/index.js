function SetSource(route)
{
    var iframe = document.getElementById('frame');
    iframe.setAttribute('src', 'html/' + route + '.html');
}

function ChangeMenuVisibility()
{
    var button = document.getElementById('menu-hider');
    var actualstatus = button.getAttribute('status');
    var menu = document.getElementById('menu');

    if(actualstatus === 'hiden')
    {
        button.setAttribute('status', 'shown');
        menu.style.display = 'flex';
    }
    if(actualstatus === 'shown')
    {
        button.setAttribute('status', 'hiden');
        menu.style.display = 'none';
    }
}

function GoToPage(page)
{
    window.location.href = page;
}