// JavaScript array of languages with their names and flag URLs
var languages = [
    { code: 'en', name: 'English', flag: 'https://flagcdn.com/us.svg' },
    { code: 'es', name: 'Spanish', flag: 'https://flagcdn.com/es.svg' },
    { code: 'fr', name: 'French', flag: 'https://flagcdn.com/fr.svg' },
    { code: 'de', name: 'German', flag: 'https://flagcdn.com/de.svg' },
    { code: 'it', name: 'Italian', flag: 'https://flagcdn.com/it.svg' },
    { code: 'ja', name: 'Japanese', flag: 'https://flagcdn.com/jp.svg' },
    { code: 'ko', name: 'Korean', flag: 'https://flagcdn.com/kr.svg' },
    { code: 'zh-CN', name: 'Chinese', flag: 'https://flagcdn.com/cn.svg' },
    { code: 'nl', name: 'Dutch', flag: 'https://flagcdn.com/nl.svg' },
    { code: 'da', name: 'Danish', flag: 'https://flagcdn.com/dk.svg' },
    { code: 'sv', name: 'Swedish', flag: 'https://flagcdn.com/se.svg' },
    { code: 'fi', name: 'Finnish', flag: 'https://flagcdn.com/fi.svg' },
    { code: 'el', name: 'Greek', flag: 'https://flagcdn.com/gr.svg' },
    { code: 'iw', name: 'Hebrew', flag: 'https://flagcdn.com/il.svg' },
    { code: 'no', name: 'Norwegian', flag: 'https://flagcdn.com/no.svg' },
    { code: 'pl', name: 'Polish', flag: 'https://flagcdn.com/pl.svg' },
    { code: 'ro', name: 'Romanian', flag: 'https://flagcdn.com/ro.svg' },
    { code: 'ru', name: 'Russian', flag: 'https://flagcdn.com/ru.svg' },
    { code: 'pt', name: 'Portuguese', flag: 'https://flagcdn.com/pt.svg' },
    { code: 'pt-BR', name: 'Brazilian', flag: 'https://flagcdn.com/br.svg' },
    { code: 'tr', name: 'Turkish', flag: 'https://flagcdn.com/tr.svg' },
    { code: 'uk', name: 'Ukrainian', flag: 'https://flagcdn.com/ua.svg' }
];


// Dynamically populate the language list using the data from languages.js
function populateLanguageList() {
    const languageList = document.getElementById('languageList');
    languages.forEach(language => {
        const li = document.createElement('li');
        li.onclick = () => setLanguage(language.code, language.name, language.flag);
        li.innerHTML = `<img src="${language.flag}" alt="${language.name}"> ${language.name}`;
        languageList.appendChild(li);
    });
}

// Google Translate Initialization
function googleTranslateElementInit() {
    new google.translate.TranslateElement({ pageLanguage: 'en' }, 'google_translate_element');
}

// Load Google Translate Element Script
(function () {
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = "//translate.google.com/translate_a/element.js?cb=googleTranslateElementInit";
    document.body.appendChild(script);
})();

// Open the modal
function openModal() {
    document.getElementById("languageModal").style.display = "flex";
}

// Close the modal
function closeModal() {
    $('#languageModal').modal('hide');
}

// Set selected language, flag, and update button display
function setLanguage(languageCode, languageName, flagUrl) {
    // Update the language and flag on the button
    document.getElementById("selectedLanguage").textContent = languageName;
    document.getElementById("selectedFlag").src = flagUrl;

    // Trigger translation using the Google Translate API
    var translateElement = new google.translate.TranslateElement({
        pageLanguage: 'en',
        includedLanguages: 'en,es,fr,de,it,ja,ko,zh-CN,nl,da,sv,fi,el,iw,no,pl,ro,ru,pt-PT,pt,tr,uk',
        layout: google.translate.TranslateElement.InlineLayout.SIMPLE
    }, 'google_translate_element');

    // Set the language and trigger translation
    var selectLanguage = document.querySelector('#google_translate_element select');
    if (selectLanguage) {
        selectLanguage.value = languageCode;
        selectLanguage.dispatchEvent(new Event('change'));
    }

    // Store the language and flag URL in cookies
    document.cookie = "lang=" + languageName + "; path=/; max-age=" + 60 * 60 * 24 * 30; // Cookie expires in 30 days
    document.cookie = "flag=" + encodeURIComponent(flagUrl) + "; path=/; max-age=" + 60 * 60 * 24 * 30;

    // Close the modal
    closeModal();
}

// Reset translation to English
function resetTranslation() {
    setLanguage('en', 'English', 'https://upload.wikimedia.org/wikipedia/commons/a/a4/Flag_of_the_United_States.svg');
}

// Check cookies on page load and set the language if set
window.onload = function () {
    debugger
    populateLanguageList();

    const cookieLang = document.cookie.split('; ').find(row => row.startsWith('lang='));
    const cookieFlag = document.cookie.split('; ').find(row => row.startsWith('flag='));

    if (cookieLang && cookieFlag) {
        const lang = cookieLang.split('=')[1];
        const flagUrl = decodeURIComponent(cookieFlag.split('=')[1]);

        // Update button based on the stored language and flag cookie
        document.getElementById("selectedLanguage").textContent = lang;
        document.getElementById("selectedFlag").src = flagUrl;

        // Trigger translation
        var translateElement = new google.translate.TranslateElement({
            pageLanguage: 'en',
            includedLanguages: 'en,es,fr,de,it,ja,ko,zh-CN,nl,da,sv,fi,el,iw,no,pl,ro,ru,pt-PT,pt,tr,uk',
            layout: google.translate.TranslateElement.InlineLayout.SIMPLE
        }, 'google_translate_element');

        var selectLanguage = document.querySelector('#google_translate_element select');
        if (selectLanguage) {
            selectLanguage.value = lang;
            selectLanguage.dispatchEvent(new Event('change'));
        }
    } else {
        // Default to English if no language cookie is set
        setLanguage('en', 'English', 'https://upload.wikimedia.org/wikipedia/commons/a/a4/Flag_of_the_United_States.svg');
    }
};
