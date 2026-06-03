var previous_button = 'home-button';

function SetSource(button, route)
{
    ActivateMenuButton(button);
    var iframe = document.getElementById('frame');
    iframe.setAttribute('src', 'html/' + route + '.html');
}

function ActivateMenuButton(button)
{
    var previous = document.getElementById(previous_button);
    if (previous)
    {
        previous.classList.remove('active');
    }

    var buttonElement = document.getElementById(button);
    if (buttonElement)
    {
        buttonElement.classList.add('active');
    }

    previous_button = button;
}