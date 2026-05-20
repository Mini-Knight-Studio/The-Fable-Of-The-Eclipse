function ChangeDisplay(memberId)
{
    var member_card = document.getElementById(memberId);
    
    var member_card_display_button = member_card.querySelector('.member-button');
    var member_card_display_button_icon = member_card.querySelector('.icon');
    var member_card_display = member_card.querySelector('.member-display');
    
    var status = member_card_display_button.getAttribute('status');

    if (status === 'off')
    {
        member_card_display_button.setAttribute('status', 'on');
        member_card_display.style.display = 'flex';
        member_card_display_button_icon.setAttribute('icon', 'material-symbols:arrow-drop-up-rounded');
    }
    else if (status === 'on')
    {
        member_card_display_button.setAttribute('status', 'off');
        member_card_display.style.display = 'none';
        member_card_display_button_icon.setAttribute('icon', 'material-symbols:arrow-drop-down-rounded');
    }
}