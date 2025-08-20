//============ Quantity Controller ============//
function IncDecBtn(elem) {
    const inputId =
        elem.getAttribute('data-input-counter-increment') ||
        elem.getAttribute('data-input-counter-decrement');

    const input = document.getElementById(inputId);
    if (!input) return;

    let value = parseInt(input.value, 10) || 1;

    if (elem.hasAttribute('data-input-counter-increment')) {
        input.value = value + 1;
    } else if (elem.hasAttribute('data-input-counter-decrement')) {
        if (value > 1) input.value = value - 1;
    }
    CalculateSummary()
}

//===== Remove Item Card Functionality =====//
function RemoveItemCard(index, productId) {
    const element = document.getElementById(`SelectedItem-${index}`);

    if (element) element.remove();
    alert();
    fetch('/Cart/RemoveFromCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(productId)
    })
        .then(response => {
            if (!response.ok) throw new Error("Failed to remove from cart.");
            return response.json();
        })
        .then(data => {
            console.log("Removed from cart:", data);
        })
        .catch(error => {
            console.error("Error removing item:", error);
        });
    CalculateSummary()
}

///== Tailwind CSS Configuration ==========//
tailwind.config = {
    darkMode: 'class',
    theme: {
        extend: {
            colors: {
                primary: {
                    50: "#eff6ff",
                    100: "#dbeafe",
                    200: "#bfdbfe",
                    300: "#93c5fd",
                    400: "#60a5fa",
                    500: "#3b82f6",
                    600: "#2563eb",
                    700: "#1d4ed8",
                    800: "#1e40af",
                    900: "#1e3a8a",
                    950: "#172554"
                }
            },
            fontFamily: {
                body: [
                    'Inter',
                    'ui-sans-serif',
                    'system-ui',
                    '-apple-system',
                    'Segoe UI',
                    'Roboto',
                    'Helvetica Neue',
                    'Arial',
                    'Noto Sans',
                    'sans-serif'
                ],
                sans: [
                    'Inter',
                    'ui-sans-serif',
                    'system-ui',
                    '-apple-system',
                    'Segoe UI',
                    'Roboto',
                    'Helvetica Neue',
                    'Arial',
                    'Noto Sans',
                    'sans-serif'
                ]
            }
        }
    }
};

//========== Oredr Summary Calculation ==========//
function CalculateSummary() {
    let total = 0;
    let count = 0;
    let discount = 0;

    const priceInputs = document.querySelectorAll('.price');
    const qtyInputs = document.querySelectorAll('.qty');
    const mutedPrice = document.querySelectorAll('#HiddenMutedPrice');
    const totalDisplay = document.getElementById('TotalPrice');
    const tax = document.getElementById('Tax');
    const TotalFinalPrice = document.getElementById('total');
    const ItemCount = document.getElementById('ItemCount');
    const Discount = document.getElementById('Discount');
    const DeliveryCharges = document.getElementById('DeliveryCharges');
    const ApplyCouponDiscount = document.getElementById('ApplyCouponDiscount');

    if (priceInputs.length > 0) {

        for (let i = 0; i < priceInputs.length; i++) {
            const price = parseFloat(priceInputs[i].value) ?? 0;
            const qty = parseFloat(qtyInputs[i].value) ?? 0;
            count += qty;
            total += (price * qty);
            discount += (qty * mutedPrice[i].value)
        }

        DeliveryCharges.textContent = 100;
        totalDisplay.textContent = total.toFixed(2);
        tax.textContent = total * 0.02;
        TotalFinalPrice.textContent = (parseInt(totalDisplay.textContent) + parseInt(tax.textContent) + parseInt(DeliveryCharges.textContent) - parseInt(ApplyCouponDiscount.value)).toFixed(2)
        ItemCount.textContent = count;
        Discount.textContent = parseInt(discount) + parseInt(ApplyCouponDiscount.value);
    }

    else {
        totalDisplay.textContent = "--";
        tax.textContent = "--";
        TotalFinalPrice.textContent = "--"
        ItemCount.textContent = "--";
        Discount.textContent = "--";
        DeliveryCharges.textContent = "--"

        showEmptyCartAnimation();

    }
}

// Recalculate on input change
document.addEventListener('input', function (e) {
    if (
        e.target.classList.contains('qty') ||
        e.target.id === 'Discount'
    ) {
        CalculateSummary();
    }
});

// Also recalculate on page load
window.addEventListener('DOMContentLoaded', () => {
    CalculateSummary();
});


//============= Show Empty Cart Animation ==========//
function showEmptyCartAnimation() {
    const container = document.getElementById('emptyCartContainer');
    if (container) {
        container.innerHTML = `
          <div class="flex justify-center my-10">
            <lottie-player
              src="/assets/empty.json"
              background="transparent"
              speed="1"
              style="width: 300px; height: 300px;"
              loop
              autoplay>
            </lottie-player>
          </div>
        `;
    }
}




//============= Method To Validate Coupon ==========
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('applyCodeBtn').addEventListener('click', async (e) => {
        e.preventDefault();

        const coupon = document.getElementById('voucher').value;

        try {
            const response = await fetch('/api/validate-coupon', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(coupon)
            });

            if (!response.ok) {
                throw new Error('Network response was not OK');
            }

            const result = await response.json();

            document.getElementById('ApplyCouponDiscount').value = result.discount;
            CalculateSummary();
            document.getElementById('CautionBox').hidden = false;
            if (!result.isValid) {
                document.getElementById('FailedMsg').hidden = false;
            } else if (result.discount === 0) {
                document.getElementById('WarningMsg').hidden = false;
            } else {
                document.getElementById('SuccessMsg').hidden = false;
            }


            setTimeout(() => {
                document.getElementById('CautionBox').hidden = true;
                document.getElementById('FailedMsg').hidden = true;
                document.getElementById('WarningMsg').hidden = true;
                document.getElementById('SuccessMsg').hidden = true;
            }, 5000);

        } catch (err) {

        }
    }); })



document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('UserCart').addEventListener('click', async (e) => {
        e.preventDefault();
        e.stopPropagation();
        try {
            const response = await fetch('/Order/PlaceOrder', {
                method: 'POST',
               
            });
            debugger;
            console.log(response);
            if (response.ok) {
                alert()
                window.location.href = `/Order/Order`;
            }
        } catch(ex) {
        }
    });
});

//============= Method To Save Carrt to DB  ==========



//=== Product Qty Relation =========
function CountProductQty() {
    let products = [];

    const productID = document.querySelectorAll('#ProductId');

    for (let i = 0; i < productID.length; i++) {
        products.push({
            ProductId: productID[i].value,
            Quantity: document.getElementById(`counter-input-${i}`).value
        });
    }
    return products ?? null;
}


