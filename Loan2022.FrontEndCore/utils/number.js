export const caculatePercent = (number, total, fractionDigits) => {
    if(!(number) || !(total)) return 0;
    if(!fractionDigits) fractionDigits = 2;
    return parseFloat((((number || 0) * 100.0) / total).toFixed(fractionDigits));    
}

export const formatter = new Intl.NumberFormat('en-US', {
    currency: 'USD',
    minimumFractionDigits: 0
})