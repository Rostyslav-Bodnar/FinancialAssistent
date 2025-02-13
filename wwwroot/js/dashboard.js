function getTotalBalance() {
    let cash = document.getElementById("inCash").textContent.replace(" ₴", "").trim();
    let card = document.getElementById("card-balance").textContent.replace(" ₴", "").trim();

    let cashValue = parseFloat(cash) || 0;
    let cardValue = parseFloat(card) || 0;

    let total = cashValue + cardValue;

    document.getElementById("total-balance").textContent = `${total} ₴`;
}
// Відкрити меню вибору віджета
function openWidgetMenu() {
    document.getElementById("widgetMenu").style.display = "block";
}

// Закрити меню вибору віджета
function closeWidgetMenu() {
    document.getElementById("widgetMenu").style.display = "none";
}

// Додавання віджета
function addWidget(type) {
    const container = document.getElementById("widgetsContainer");

    const widget = document.createElement("div");
    widget.classList.add("widget");
    widget.dataset.type = type;

    switch (type) {
        case "balanceChart":
            widget.innerHTML = `<h3>Balance Chart</h3><div id="chart${Date.now()}" class="chart"></div>`;
            break;
        case "expenseTable":
            widget.innerHTML = `<h3>Expense Table</h3><p>List of recent expenses...</p>`;
            break;
        case "categoryBreakdown":
            widget.innerHTML = `<h3>Category Breakdown</h3><p>Expenses by category...</p>`;
            break;
        default:
            widget.innerHTML = `<h3>Unknown Widget</h3>`;
    }

    const deleteBtn = document.createElement("button");
    deleteBtn.innerText = "×";
    deleteBtn.classList.add("delete-widget");
    deleteBtn.onclick = () => {
        widget.remove();
        saveWidgets();
    };

    widget.appendChild(deleteBtn);
    container.appendChild(widget);
    closeWidgetMenu();
    saveWidgets();
}

// Збереження віджетів у LocalStorage
function saveWidgets() {
    const widgets = Array.from(document.querySelectorAll(".widget")).map(w => w.dataset.type);
    localStorage.setItem("widgets", JSON.stringify(widgets));
}

// Завантаження віджетів із LocalStorage
function loadWidgets() {
    const widgets = JSON.parse(localStorage.getItem("widgets")) || [];
    widgets.forEach(type => addWidget(type));
}

// Додавання Drag-and-Drop
new Sortable(document.getElementById("widgetsContainer"), {
    animation: 150,
    onEnd: saveWidgets
});

// Виклик при завантаженні сторінки
window.onload = loadWidgets;
getTotalBalance();