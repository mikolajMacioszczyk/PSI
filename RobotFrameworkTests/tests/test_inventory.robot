*** Settings ***
#Variables    resources/variables.py
#Library      resources/custom_library.py
Library      SeleniumLibrary
#Resource     resources/common.resource
#Resource     resources/some_other.resource
#Suite Setup  Setup Suite
#Test Setup   Setup Test

*** Variables ***
${USERNAME}    admin@gmail.com
${USERNAME_2}    admin
${PASSWORD}    admin
${SUBMIT_LOGIN}    kc-login
${URL}    http://localhost:4200
${HOME_PAGE_TEXT}    Logowanie jako Admin
${LINK}    //a[normalize-space()='Powiadomienia']
${EDYTUJ_POWIADOMIENIE}    //body//app-root//div[@class='col-md-9']//div//div//div[1]//div[2]//button[1]
${TRESHHOLD}    //input[@id='minimumStock']
${TEXT}    //textarea[@id='message']
${SAVE}    //button[normalize-space()='Zapisz']
${KEYCLOAK_LINK}    http://localhost:8001

#robot -d Results ./tests/test_inventory.robot

*** Test Cases ***
Notification Edit Admin Test Case
    [Documentation]    Test case to modify notifications Admin
    Open Browser        ${URL}    chrome
    Maximize Browser Window
    wait until page contains    ${HOME_PAGE_TEXT}
    click button    ${HOME_PAGE_TEXT}
    wait until page contains    Sign in to your account
    Input Text    id=username    ${USERNAME}
    Input Text    id=password    ${PASSWORD}
    Click Button    id=${SUBMIT_LOGIN}
    wait until page contains    ${HOME_PAGE_TEXT}
    click button    ${HOME_PAGE_TEXT}
    wait until page contains    Witaj Admin!
    Click Element    xpath=${LINK}
    wait until page contains    Magazyny
    click button     Warehouse A
    wait until page contains    SKU: 00000000-0000-0000-0000-000000000092
    click element    xpath=${EDYTUJ_POWIADOMIENIE}
    Input Text    xpath=${TRESHHOLD}    10
    Input Text    xpath=${TEXT}    robot framework wpisał 10
    click element    xpath=${SAVE}
    Go To    ${KEYCLOAK_LINK}
    wait until page contains    Sign in to your account
    Input Text    id=username    ${USERNAME_2}
    Input Text    id=password    ${PASSWORD}
    Click Button    id=${SUBMIT_LOGIN}
    wait until page contains    Welcome to Keycloak
    click element    xpath=//div[@class='pf-v5-l-stack__item pf-m-fill']
    click element    xpath=//li[2]//button[1]
    wait until page contains    Welcome to
    click element    xpath=//a[@id='nav-item-users']
    wait until page contains    Users
    sleep    1s
    click link    admin@gmail.com
    sleep    1s
    wait until page contains    General
    Click Element    xpath=//span[normalize-space()='Sessions']
    wait until page contains    Started
    click element    xpath=//button[normalize-space()='Logout all sessions']
    wait until page contains    Logout all sessions
    click button    id=modal-confirm
    wait until page contains    No sessions
    click element    xpath=//button[@id='user-dropdown']//span[@class='pf-v5-c-menu-toggle__toggle-icon']//*[name()='svg']//*[name()='path' and contains(@d,'M31.3 192h')]
    click element    xpath=//span[contains(text(),'Sign out')]
    wait until page contains    Sign in to your account
    sleep    1s
    close browser

Warehouse Usage Test Case
    [Documentation]    Test case to check if WarehouseEmployee works properly
    Open Browser        ${URL}    chrome
    Maximize Browser Window
    wait until page contains    ${HOME_PAGE_TEXT}
    click button    Logowanie jako Warehouse Employee
    wait until page contains    Sign in to your account
    Input Text    id=username    magazyn@gmail.com
    Input Text    id=password    magazyn
    Click Button    id=${SUBMIT_LOGIN}
    wait until page contains    ${HOME_PAGE_TEXT}
    click button    Logowanie jako Warehouse Employee
    wait until page contains    Witaj
    Click Element    xpath=//a[normalize-space()='Magazyn']
    wait until page contains    Magazyny
    click button     Warehouse A
    wait until page contains    SKU: 00000000-0000-0000-0000-000000000092
    click element    xpath=/html[1]/body[1]/app-root[1]/app-warehouse-employee-screen[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/button[2]
    click element    xpath=//body//app-root//div[@class='col-md-9']//div//div//div[1]//div[1]//div[1]//div[2]//button[3]
    sleep    2s
    wait until page contains element    //div[@aria-label='Niski stan magazynowy']
    Go To    ${KEYCLOAK_LINK}
    wait until page contains    Sign in to your account
    Input Text    id=username    ${USERNAME_2}
    Input Text    id=password    ${PASSWORD}
    Click Button    id=${SUBMIT_LOGIN}
    wait until page contains    Welcome to Keycloak
    click element    xpath=//div[@class='pf-v5-l-stack__item pf-m-fill']
    click element    xpath=//li[2]//button[1]
    wait until page contains    Welcome to
    click element    xpath=//a[@id='nav-item-users']
    wait until page contains    Users
    sleep    1s
    click link    magazyn@gmail.com
    sleep    1s
    wait until page contains    General
    Click Element    xpath=//span[normalize-space()='Sessions']
    wait until page contains    Started
    click element    xpath=//button[normalize-space()='Logout all sessions']
    wait until page contains    Logout all sessions
    click button    id=modal-confirm
    wait until page contains    No sessions
    click element    xpath=//button[@id='user-dropdown']//span[@class='pf-v5-c-menu-toggle__toggle-icon']//*[name()='svg']//*[name()='path' and contains(@d,'M31.3 192h')]
    click element    xpath=//span[contains(text(),'Sign out')]
    wait until page contains    Sign in to your account
    sleep    1s
    close browser

*** Keywords ***

